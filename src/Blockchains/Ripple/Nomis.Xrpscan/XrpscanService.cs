// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Domain.Scoring.Entities;
using Nomis.Greysafe.Interfaces;
using Nomis.Greysafe.Interfaces.Contracts;
using Nomis.Greysafe.Interfaces.Responses;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces.Models;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;
using Nomis.Xrpscan.Calculators;
using Nomis.Xrpscan.Interfaces;
using Nomis.Xrpscan.Settings;

namespace Nomis.Xrpscan
{
    /// <inheritdoc cref="XrpscanService"/>
    internal sealed class XrpscanService :
        BlockchainDescriptor,
        IRippleScoringService,
        ITransientService
    {
        private readonly IXrpscanClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IDefiLlamaService _defiLlamaService;
        private readonly IGreysafeService _greysafeService;
        private readonly ILogger<XrpscanService> _logger;

        /// <summary>
        /// Initialize <see cref="XrpscanService"/>.
        /// </summary>
        /// <param name="settings"><see cref="XrpscanSettings"/>.</param>
        /// <param name="client"><see cref="IXrpscanClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="greysafeService"><see cref="IGreysafeService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public XrpscanService(
            IOptions<XrpscanSettings> settings,
            IXrpscanClient client,
            ICoingeckoService coingeckoService,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService,
            IDefiLlamaService defiLlamaService,
            IGreysafeService greysafeService,
            ILogger<XrpscanService> logger)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _defiLlamaService = defiLlamaService;
            _greysafeService = greysafeService;
            _logger = logger;
        }

        /// <summary>
        /// Coingecko native token id.
        /// </summary>
        public string CoingeckoNativeTokenId => "ripple";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            var account = await _client.GetAccountDataAsync(request.Address).ConfigureAwait(false);
            decimal.TryParse(account.XrpBalance, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out decimal balance);

            var kycStatus = await _client.GetKycDataAsync(request.Address).ConfigureAwait(false);
            var assets = (await _client.GetAssetsDataAsync(request.Address).ConfigureAwait(false)).ToList();
            var orders = await _client.GetOrdersDataAsync(request.Address).ConfigureAwait(false);
            var obligations = await _client.GetObligationsDataAsync(request.Address).ConfigureAwait(false);
            decimal usdBalance =
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * balance ?? 0;
            var transactions = await _client.GetTransactionsDataAsync(request.Address).ConfigureAwait(false);
            var nfts = await _client.GetNftsDataAsync(request.Address).ConfigureAwait(false);

            #region Greysafe scam reports

            GreysafeReportsResponse? greysafeReportsResponse = null;
            if ((request as IWalletGreysafeRequest)?.GetGreysafeData == true)
            {
                try
                {
                    greysafeReportsResponse = (await _greysafeService.GetWalletReportsAsync(request.Address).ConfigureAwait(false)).Data;
                }
                catch (NoDataException)
                {
                    // ignored
                }
            }

            #endregion Greysafe scam reports

            #region Tokens balances

            var tokenBalances = new List<TokenBalanceData>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                var balances = assets
                    .Select(t => new TokenBalanceData(
                        new TokenPriceData
                        {
                            Confidence = 0,
                            Decimals = null,
                            Timestamp = (ulong)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                            Price = 0,
                            Symbol = t.Currency
                        },
                        t.Counterparty!,
                        null,
                        t.Value?.ToBigInteger()));
                tokenBalances.AddRange(balances);
            }

            #endregion Tokens balances

            var walletStats = new RippleStatCalculator(
                    account,
                    balance,
                    usdBalance,
                    kycStatus,
                    assets,
                    orders,
                    obligations,
                    transactions,
                    nfts,
                    tokenBalances,
                    greysafeReportsResponse?.Reports)
                .GetStats() as TWalletStats;

            double score = walletStats!.GetScore<TWalletStats, TTransactionIntervalData>();
            var scoringData = new ScoringData(request.Address, request.Address, ChainId, score, JsonSerializer.Serialize(walletStats));
            await _scoringService.SaveScoringDataToDatabaseAsync(scoringData, cancellationToken).ConfigureAwait(false);

            // getting signature
            ushort mintedScore = (ushort)(score * 10000);
            var signatureResult = await Result<SoulboundTokenSignature>.FailAsync(
                new SoulboundTokenSignature
                {
                    Signature = null
                }, "Get token signature: Can't get signature without Greysafe adjusting score.").ConfigureAwait(false);
            if ((request as IWalletGreysafeRequest)?.GetGreysafeData == true)
            {
                signatureResult = _soulboundTokenService.GetSoulboundTokenSignature(new()
                {
                    Score = mintedScore,
                    ScoreType = request.ScoreType,
                    To = request.Address,
                    Nonce = request.Nonce,
                    ChainId = ChainId,
                    ContractAddress = SBTContractAddresses?.ContainsKey(request.ScoreType) == true ? SBTContractAddresses?[request.ScoreType] : null,
                    Deadline = request.Deadline
                });
            }

            var messages = signatureResult.Messages;
            messages.Add($"Got {ChainName} wallet {request.ScoreType.ToString()} score.");
            return await Result<TWalletScore>.SuccessAsync(
                new()
                {
                    Address = request.Address,
                    Stats = walletStats,
                    Score = score,
                    MintedScore = mintedScore,
                    Signature = signatureResult.Data.Signature
                }, messages).ConfigureAwait(false);
        }
    }
}