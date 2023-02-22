// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.DefiLlama.Interfaces.Contracts;
using Nomis.Dex.Abstractions.Enums;
using Nomis.DexProviderService.Interfaces;
using Nomis.DexProviderService.Interfaces.Extensions;
using Nomis.DexProviderService.Interfaces.Requests;
using Nomis.Domain.Scoring.Entities;
using Nomis.Greysafe.Interfaces;
using Nomis.Greysafe.Interfaces.Contracts;
using Nomis.Greysafe.Interfaces.Responses;
using Nomis.HapiExplorer.Interfaces;
using Nomis.HapiExplorer.Interfaces.Contracts;
using Nomis.HapiExplorer.Interfaces.Responses;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces.Models;
using Nomis.Tronscan.Calculators;
using Nomis.Tronscan.Interfaces;
using Nomis.Tronscan.Interfaces.Models;
using Nomis.Tronscan.Settings;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.Tronscan
{
    /// <inheritdoc cref="ITronScoringService"/>
    internal sealed class TronscanService :
        BlockchainDescriptor,
        ITronScoringService,
        IHasDefiLlamaIntegration,
        ITransientService
    {
        private readonly ITronscanClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IDefiLlamaService _defiLlamaService;
        private readonly IHapiExplorerService _hapiExplorerService;
        private readonly IGreysafeService _greysafeService;

        /// <summary>
        /// Initialize <see cref="TronscanService"/>.
        /// </summary>
        /// <param name="settings"><see cref="TronscanSettings"/>.</param>
        /// <param name="client"><see cref="ITronscanClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="hapiExplorerService"><see cref="IHapiExplorerService"/>.</param>
        /// <param name="greysafeService"><see cref="IGreysafeService"/>.</param>
        public TronscanService(
            IOptions<TronscanSettings> settings,
            ITronscanClient client,
            ICoingeckoService coingeckoService,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService,
            IDexProviderService dexProviderService,
            IDefiLlamaService defiLlamaService,
            IHapiExplorerService hapiExplorerService,
            IGreysafeService greysafeService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _dexProviderService = dexProviderService;
            _defiLlamaService = defiLlamaService;
            _hapiExplorerService = hapiExplorerService;
            _greysafeService = greysafeService;
        }

        /// <inheritdoc />
        public string DefiLLamaChainId => "tron";

        /// <inheritdoc />
        public string CoingeckoNativeTokenId => "tron";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            var accountData = await _client.GetBalanceAsync(request.Address).ConfigureAwait(false);
            decimal balance = accountData.Tokens?.Sum(x => x.Amount) ?? accountData.Balance;
            decimal usdBalance =
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * balance ?? 0;
            var contractsData = await _client.GetContractsAsync(request.Address).ConfigureAwait(false);
            var transactions = (await _client.GetTransactionsAsync<TronscanAccountNormalTransactions, TronscanAccountNormalTransaction>(request.Address).ConfigureAwait(false)).ToList();
            var internalTransactions = (await _client.GetTransactionsAsync<TronscanAccountInternalTransactions, TronscanAccountInternalTransaction>(request.Address).ConfigureAwait(false)).ToList();
            var transfers = await _client.GetTransactionsAsync<TronscanAccountTransfers, TronscanAccountTransfer>(request.Address).ConfigureAwait(false);
            var tokens = transfers.Where(x => x.TokenInfo?.TokenType?.Equals("trc721", StringComparison.OrdinalIgnoreCase) == true).ToList();

            #region HAPI protocol

            HapiProxyRiskScoreResponse? hapiRiskScore = null;
            if ((request as IWalletHapiProtocolRequest)?.GetHapiProtocolData == true)
            {
                try
                {
                    hapiRiskScore = (await _hapiExplorerService.GetWalletRiskScoreAsync("tron", request.Address).ConfigureAwait(false)).Data;
                }
                catch (NoDataException)
                {
                    // ignored
                }
            }

            #endregion HAPI protocol

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

            #region Tokens data

            var tokensData = new List<(string TokenContractId, string? TokenContractIdWithBlockchain, BigInteger? Balance)>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                foreach (var accountToken in accountData.Tokens?.DistinctBy(t => t.TokenId) ?? new List<TronscanAccountToken>())
                {
                    var tokenBalance = accountToken.Balance?.ToBigInteger();
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((accountToken.TokenId!, $"{DefiLLamaChainId}:{accountToken.TokenId}", tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var dexTokensData = await _dexProviderService.TokensDataAsync(new TokensDataRequest
            {
                Blockchain = Chain.Tron,
                IncludeUniversalTokenLists = true,
                FromCache = true
            }).ConfigureAwait(false);

            var tokenBalances = await _defiLlamaService
                .TokensBalancesAsync(request as IWalletTokensBalancesRequest, tokensData, dexTokensData.Data).ConfigureAwait(false);

            #endregion Tokens balances

            var walletStats = new TronStatCalculator(
                    request.Address,
                    accountData,
                    balance,
                    usdBalance,
                    transactions,
                    internalTransactions,
                    tokens,
                    accountData.Trc20Balances ?? new List<TronscanAccountTrc20TokenBalance>(),
                    contractsData,
                    tokenBalances,
                    hapiRiskScore,
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
                }, "Get token signature: Can't get signature without HAPI Protocol and Greysafe adjusting score.").ConfigureAwait(false);
            if ((request as IWalletHapiProtocolRequest)?.GetHapiProtocolData == true
                && (request as IWalletGreysafeRequest)?.GetGreysafeData == true)
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