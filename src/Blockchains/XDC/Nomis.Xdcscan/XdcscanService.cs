// ------------------------------------------------------------------------------------------------------
// <copyright file="XdcscanService.cs" company="Nomis">
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
using Nomis.Chainanalysis.Interfaces;
using Nomis.Chainanalysis.Interfaces.Contracts;
using Nomis.Chainanalysis.Interfaces.Responses;
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
using Nomis.Xdcscan.Calculators;
using Nomis.Xdcscan.Interfaces;
using Nomis.Xdcscan.Interfaces.Enums;
using Nomis.Xdcscan.Interfaces.Models;
using Nomis.Xdcscan.Settings;

namespace Nomis.Xdcscan
{
    /// <inheritdoc cref="IXdcScoringService"/>
    internal sealed class XdcscanService :
        BlockchainDescriptor,
        IXdcScoringService,
        IHasDefiLlamaIntegration,
        ITransientService
    {
        private readonly IXdcscanClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly IEvmSoulboundTokenService _soulboundTokenService;
        private readonly ISnapshotService _snapshotService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IDefiLlamaService _defiLlamaService;
        private readonly IGreysafeService _greysafeService;
        private readonly IChainanalysisService _chainanalysisService;

        /// <summary>
        /// Initialize <see cref="XdcscanService"/>.
        /// </summary>
        /// <param name="settings"><see cref="XdcscanSettings"/>.</param>
        /// <param name="client"><see cref="IXdcscanClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="IEvmSoulboundTokenService"/>.</param>
        /// <param name="snapshotService"><see cref="ISnapshotService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="greysafeService"><see cref="IGreysafeService"/>.</param>
        /// <param name="chainanalysisService"><see cref="IChainanalysisService"/>.</param>
        public XdcscanService(
            IOptions<XdcscanSettings> settings,
            IXdcscanClient client,
            ICoingeckoService coingeckoService,
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
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _snapshotService = snapshotService;
            _dexProviderService = dexProviderService;
            _defiLlamaService = defiLlamaService;
            _greysafeService = greysafeService;
            _chainanalysisService = chainanalysisService;
        }

        /// <inheritdoc />
        public string DefiLLamaChainId => "xdc";

        /// <inheritdoc />
        public string CoingeckoNativeTokenId => "xdce-crowd-sale";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            if (request.Address.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                request.Address = request.Address.Replace("0x", "xdc", StringComparison.OrdinalIgnoreCase);
            }

            var accountData = await _client.GetAccountDataAsync(request.Address).ConfigureAwait(false);
            decimal usdBalance =
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * accountData.BalanceNumber ?? 0;
            var transactions = (await _client.GetTransactionsAsync<XdcscanAccountNormalTransactions, XdcscanAccountNormalTransaction>(request.Address).ConfigureAwait(false)).ToList();
            var internalTransactions = (await _client.GetTransactionsAsync<XdcscanAccountInternalTransactions, XdcscanAccountInternalTransaction>(request.Address).ConfigureAwait(false)).ToList();

            var xrc20TokenTransactions = accountData.HasXrc20
                ? (await _client.GetTransactionsAsync<XdcscanAccountXRC20TokenEvents, XdcscanAccountXRC20TokenEvent>(request.Address).ConfigureAwait(false)).ToList()
                : new();

            long holderNftCount = (await _client.GetHolderTokensDataAsync(request.Address, XdcscanTokenType.Xrc721).ConfigureAwait(false)).Total
                            + (await _client.GetHolderTokensDataAsync(request.Address, XdcscanTokenType.Xrc1155).ConfigureAwait(false)).Total;
            var xrc721TokenTransactions = accountData.HasXrc721
                ? (await _client.GetTransactionsAsync<XdcscanAccountXRC721TokenEvents, XdcscanAccountXRC721TokenEvent>(request.Address).ConfigureAwait(false)).ToList()
                : new();

            var xrc1155TokenTransactions = accountData.HasXrc1155
                ? (await _client.GetTransactionsAsync<XdcscanAccountXRC1155TokenEvents, XdcscanAccountXRC1155TokenEvent>(request.Address).ConfigureAwait(false)).ToList()
                : new();

            var nftTokenTransactions = new List<IXdcscanAccountNftTokenEvent>();
            nftTokenTransactions.AddRange(xrc721TokenTransactions);
            nftTokenTransactions.AddRange(xrc1155TokenTransactions);

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
                var tokens = await _client.GetTokensAsync(request.Address).ConfigureAwait(false);
                foreach (var token in tokens.DistinctBy(t => t.Token))
                {
                    var tokenBalance = token.Quantity?.ToBigInteger();
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((token.Token!, $"{DefiLLamaChainId}:{token.Token?.Replace("xdc", "0x", StringComparison.OrdinalIgnoreCase)}", tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var dexTokensData = await _dexProviderService.TokensDataAsync(new TokensDataRequest
            {
                Blockchain = Chain.XDC,
                IncludeUniversalTokenLists = true,
                FromCache = true
            }).ConfigureAwait(false);

            var tokenBalances = await _defiLlamaService
                .TokensBalancesAsync(request as IWalletTokensBalancesRequest, tokensData, dexTokensData.Data).ConfigureAwait(false);

            #endregion Tokens balances

            var walletStats = new XdcStatCalculator(
                    request.Address,
                    accountData,
                    usdBalance,
                    transactions,
                    internalTransactions,
                    nftTokenTransactions,
                    xrc20TokenTransactions,
                    holderNftCount,
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
    }
}