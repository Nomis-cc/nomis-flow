// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nethereum.Util;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Chainanalysis.Interfaces;
using Nomis.Chainanalysis.Interfaces.Contracts;
using Nomis.Chainanalysis.Interfaces.Responses;
using Nomis.CscExplorer.Calculators;
using Nomis.CscExplorer.Interfaces;
using Nomis.CscExplorer.Interfaces.Models;
using Nomis.CscExplorer.Responses;
using Nomis.CscExplorer.Settings;
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
using Nomis.ScoringService.Interfaces;
using Nomis.Snapshot.Interfaces;
using Nomis.Snapshot.Interfaces.Contracts;
using Nomis.Snapshot.Interfaces.Models;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces.Models;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.CscExplorer
{
    /// <inheritdoc cref="ICscScoringService"/>
    internal sealed class CscExplorerService :
        BlockchainDescriptor,
        ICscScoringService,
        IHasDefiLlamaIntegration,
        ITransientService
    {
        private readonly ICscExplorerClient _client;
        private readonly IScoringService _scoringService;
        private readonly IEvmSoulboundTokenService _soulboundTokenService;
        private readonly ISnapshotService _snapshotService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IDefiLlamaService _defiLlamaService;
        private readonly IGreysafeService _greysafeService;
        private readonly IChainanalysisService _chainanalysisService;
        private readonly HttpClient _coinexClient;

        /// <summary>
        /// Initialize <see cref="CscExplorerService"/>.
        /// </summary>
        /// <param name="settings"><see cref="CscExplorerSettings"/>.</param>
        /// <param name="client"><see cref="ICscExplorerClient"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="IEvmSoulboundTokenService"/>.</param>
        /// <param name="snapshotService"><see cref="ISnapshotService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="greysafeService"><see cref="IGreysafeService"/>.</param>
        /// <param name="chainanalysisService"><see cref="IChainanalysisService"/>.</param>
        public CscExplorerService(
            IOptions<CscExplorerSettings> settings,
            ICscExplorerClient client,
            IScoringService scoringService,
            IEvmSoulboundTokenService soulboundTokenService,
            ISnapshotService snapshotService,
            IDexProviderService dexProviderService,
            IDefiLlamaService defiLlamaService,
            IGreysafeService greysafeService,
            IChainanalysisService chainanalysisService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _snapshotService = snapshotService;
            _dexProviderService = dexProviderService;
            _defiLlamaService = defiLlamaService;
            _greysafeService = greysafeService;
            _chainanalysisService = chainanalysisService;
            _coinexClient = new()
            {
                BaseAddress = new("https://www.coinex.net/")
            };
        }

        /// <inheritdoc />
        public string DefiLLamaChainId => "csc";

        /// <inheritdoc />
        public string CoingeckoNativeTokenId => "coinex-token";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            if (!new AddressUtil().IsValidAddressLength(request.Address) || !new AddressUtil().IsValidEthereumAddressHexFormat(request.Address))
            {
                throw new InvalidAddressException(request.Address);
            }

            string? balance = (await _client.GetBalanceAsync(request.Address).ConfigureAwait(false)).Data?.Balance;
            decimal.TryParse(balance, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out decimal balanceValue);
            decimal usdBalance = await GetUsdBalanceAsync(balanceValue).ConfigureAwait(false);
            var transactions = (await _client.GetTransactionsAsync<CscExplorerAccountTransactions, CscExplorerAccountTransactionData, CscExplorerAccountTransactionRecord>(request.Address).ConfigureAwait(false)).ToList();
            var cetTransfers = (await _client.GetTransactionsAsync<CscExplorerAccountCetTransfers, CscExplorerAccountCetTransferData, CscExplorerAccountCetTransferRecord>(request.Address).ConfigureAwait(false)).ToList();
            var crc20Transfers = (await _client.GetTransactionsAsync<CscExplorerAccountCrc20Transfers, CscExplorerAccountCrc20TransferData, CscExplorerAccountCrc20TransferRecord>(request.Address).ConfigureAwait(false)).ToList();
            var crc721Transfers = (await _client.GetTransactionsAsync<CscExplorerAccountCrc721Transfers, CscExplorerAccountCrc721TransferData, CscExplorerAccountCrc721TransferRecord>(request.Address).ConfigureAwait(false)).ToList();
            var tokens = (await _client.GetTokensAsync(request.Address).ConfigureAwait(false)).Data?.Crc20List ?? new List<CscExplorerTokensDataItem>();

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

            #region Chainanalysis sanctions reports

            ChainanalysisReportsResponse? chainanalysisReportsResponse = null;
            if ((request as IWalletChainanalysisRequest)?.GetChainanalysisData == true)
            {
                try
                {
                    chainanalysisReportsResponse = (await _chainanalysisService.GetWalletReportsAsync(request.Address).ConfigureAwait(false)).Data;
                }
                catch (NoDataException)
                {
                    // ignored
                }
            }

            #endregion Chainanalysis sanctions reports

            #region Snapshot protocol

            List<SnapshotVote>? snapshotVotes = null;
            List<SnapshotProposal>? snapshotProposals = null;
            if ((request as IWalletSnapshotProtocolRequest)?.GetSnapshotProtocolData == true)
            {
                snapshotVotes = (await _snapshotService.GetSnapshotVotesAsync(new()
                {
                    Voter = request.Address,
                    ChainId = ChainId
                }).ConfigureAwait(false)).Data;
                snapshotProposals = (await _snapshotService.GetSnapshotProposalsAsync(new()
                {
                    Author = request.Address,
                    ChainId = ChainId
                }).ConfigureAwait(false)).Data;
            }

            #endregion Snapshot protocol

            #region Tokens data

            var tokensData = new List<(string TokenContractId, string? TokenContractIdWithBlockchain, BigInteger? Balance)>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                var tokensWithBalance = tokens
                    .Select(x => new
                    {
                        TokenContract = x.TokenInfo?.Contract,
                        x.Balance
                    })
                    .DistinctBy(x => x.TokenContract);
                foreach (var tokenWithBalance in tokensWithBalance)
                {
                    var tokenBalance = tokenWithBalance.Balance?.ToBigInteger();
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((tokenWithBalance.TokenContract!, $"{DefiLLamaChainId}:{tokenWithBalance.TokenContract}", tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var dexTokensData = await _dexProviderService.TokensDataAsync(new TokensDataRequest
            {
                Blockchain = Chain.CSC,
                IncludeUniversalTokenLists = true,
                FromCache = true
            }).ConfigureAwait(false);

            var tokenBalances = await _defiLlamaService
                .TokensBalancesAsync(request as IWalletTokensBalancesRequest, tokensData, dexTokensData.Data).ConfigureAwait(false);

            #endregion Tokens balances

            var walletStats = new CscStatCalculator(
                    request.Address,
                    balanceValue,
                    usdBalance,
                    transactions,
                    cetTransfers,
                    crc20Transfers,
                    crc721Transfers,
                    snapshotVotes,
                    snapshotProposals,
                    tokenBalances,
                    greysafeReportsResponse?.Reports,
                    chainanalysisReportsResponse?.Identifications)
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
                }, "Get token signature: Can't get signature without Greysafe and Chainanalysis adjusting score.").ConfigureAwait(false);
            if ((request as IWalletGreysafeRequest)?.GetGreysafeData == true
                && (request as IWalletChainanalysisRequest)?.GetChainanalysisData == true)
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

        private async Task<decimal> GetUsdBalanceAsync(decimal balance)
        {
            var response = await _coinexClient.GetAsync("/res/exchange/usd").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<CoinexCscUsdPriceResponse>().ConfigureAwait(false);
            if (data?.Data?.Any() == true
                && data.Data.ContainsKey("0x0000000000000000000000000000000000000000")
                && decimal.TryParse(data.Data["0x0000000000000000000000000000000000000000"], NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." }, out decimal usdBalance))
            {
                return balance * usdBalance;
            }

            return 0;
        }
    }
}