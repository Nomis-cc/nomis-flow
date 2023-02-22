// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
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
using Nomis.Klaytnscope.Calculators;
using Nomis.Klaytnscope.Interfaces;
using Nomis.Klaytnscope.Interfaces.Extensions;
using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Klaytnscope.Settings;
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

namespace Nomis.Klaytnscope
{
    /// <inheritdoc cref="IKlaytnScoringService"/>
    internal sealed class KlaytnscopeService :
        BlockchainDescriptor,
        IKlaytnScoringService,
        IHasDefiLlamaIntegration,
        ITransientService
    {
        private readonly IKlaytnscopeClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly IEvmSoulboundTokenService _soulboundTokenService;
        private readonly ISnapshotService _snapshotService;
        private readonly IDefiLlamaService _defiLlamaService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IGreysafeService _greysafeService;
        private readonly IChainanalysisService _chainanalysisService;

        /// <summary>
        /// Initialize <see cref="KlaytnscopeService"/>.
        /// </summary>
        /// <param name="settings"><see cref="KlaytnscopeSettings"/>.</param>
        /// <param name="client"><see cref="IKlaytnscopeClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="IEvmSoulboundTokenService"/>.</param>
        /// <param name="snapshotService"><see cref="ISnapshotService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="greysafeService"><see cref="IGreysafeService"/>.</param>
        /// <param name="chainanalysisService"><see cref="IChainanalysisService"/>.</param>
        public KlaytnscopeService(
            IOptions<KlaytnscopeSettings> settings,
            IKlaytnscopeClient client,
            ICoingeckoService coingeckoService,
            IScoringService scoringService,
            IEvmSoulboundTokenService soulboundTokenService,
            ISnapshotService snapshotService,
            IDefiLlamaService defiLlamaService,
            IDexProviderService dexProviderService,
            IGreysafeService greysafeService,
            IChainanalysisService chainanalysisService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _snapshotService = snapshotService;
            _defiLlamaService = defiLlamaService;
            _dexProviderService = dexProviderService;
            _greysafeService = greysafeService;
            _chainanalysisService = chainanalysisService;
        }

        /// <inheritdoc />
        public string DefiLLamaChainId => "klaytn";

        /// <inheritdoc />
        public string CoingeckoNativeTokenId => "klay-token";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            string? hexBalance = (await _client.GetBalanceAsync(request.Address).ConfigureAwait(false)).Result?.Balance?.Replace("0x", string.Empty, StringComparison.OrdinalIgnoreCase);
            if (hexBalance?.Length % 2 != 0)
            {
                hexBalance = $"0{hexBalance}";
            }

            BigInteger.TryParse(hexBalance, NumberStyles.HexNumber, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out var balanceWei);
            decimal usdBalance =
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * balanceWei.ToKlay() ?? 0;
            var kip17Tokens = (await _client.GetTransactionsAsync<KlaytnscopeAccountKIP17TokenEvents, KlaytnscopeAccountKIP17TokenEvent>(request.Address).ConfigureAwait(false)).ToList();
            var nftTransfers = (await _client.GetTransactionsAsync<KlaytnscopeAccountNftTransfers, KlaytnscopeAccountNftTransfer>(request.Address).ConfigureAwait(false)).ToList();
            var transactions = (await _client.GetTransactionsAsync<KlaytnscopeAccountNormalTransactions, KlaytnscopeAccountNormalTransaction>(request.Address).ConfigureAwait(false)).ToList();
            var internalTransactions = (await _client.GetTransactionsAsync<KlaytnscopeAccountInternalTransactions, KlaytnscopeAccountInternalTransaction>(request.Address).ConfigureAwait(false)).ToList();
            var holdTokenBalances = await _client.GetTokenBalancesAsync(request.Address).ConfigureAwait(false);

            // var kip37Tokens = (await Client.GetTransactionsAsync<KlaytnscopeAccountKIP37TokenEvents, KlaytnscopeAccountKIP37TokenEvent>(address)).ToList();

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
                foreach (var holdTokenBalance in holdTokenBalances.Result.DistinctBy(x => x.TokenAddress))
                {
                    var tokenBalance = holdTokenBalance.Amount?.ToBigInteger();
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((holdTokenBalance.TokenAddress!, $"{DefiLLamaChainId}:{holdTokenBalance.TokenAddress}", tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var dexTokensData = await _dexProviderService.TokensDataAsync(new TokensDataRequest
            {
                Blockchain = Chain.Klaytn,
                IncludeUniversalTokenLists = true,
                FromCache = true
            }).ConfigureAwait(false);

            var tokenBalances = await _defiLlamaService
                .TokensBalancesAsync(request as IWalletTokensBalancesRequest, tokensData, dexTokensData.Data).ConfigureAwait(false);

            #endregion Tokens balances

            var walletStats = new KlaytnStatCalculator(
                    request.Address,
                    (decimal)balanceWei,
                    usdBalance,
                    transactions,
                    internalTransactions,
                    nftTransfers,
                    kip17Tokens,
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