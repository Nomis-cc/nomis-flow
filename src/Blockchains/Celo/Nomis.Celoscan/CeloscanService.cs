// ------------------------------------------------------------------------------------------------------
// <copyright file="CeloscanService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Net;
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
using Nomis.Celoscan.Calculators;
using Nomis.Celoscan.Interfaces;
using Nomis.Celoscan.Interfaces.Extensions;
using Nomis.Celoscan.Interfaces.Models;
using Nomis.Celoscan.Settings;
using Nomis.Chainanalysis.Interfaces;
using Nomis.Chainanalysis.Interfaces.Contracts;
using Nomis.Chainanalysis.Interfaces.Responses;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.DefiLlama.Interfaces.Contracts;
using Nomis.Dex.Abstractions.Contracts;
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
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.SoulboundTokenService.Interfaces.Models;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.Celoscan
{
    /// <inheritdoc cref="ICeloScoringService"/>
    internal sealed class CeloscanService :
        BlockchainDescriptor,
        ICeloScoringService,
        IHasDefiLlamaIntegration,
        ITransientService
    {
        private readonly ICeloscanClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly IEvmSoulboundTokenService _soulboundTokenService;
        private readonly ISnapshotService _snapshotService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IDefiLlamaService _defiLlamaService;
        private readonly IGreysafeService _greysafeService;
        private readonly IChainanalysisService _chainanalysisService;

        /// <summary>
        /// Initialize <see cref="CeloscanService"/>.
        /// </summary>
        /// <param name="settings"><see cref="CeloscanSettings"/>.</param>
        /// <param name="client"><see cref="ICeloscanClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="IEvmSoulboundTokenService"/>.</param>
        /// <param name="snapshotService"><see cref="ISnapshotService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="greysafeService"><see cref="IGreysafeService"/>.</param>
        /// <param name="chainanalysisService"><see cref="IChainanalysisService"/>.</param>
        public CeloscanService(
            IOptions<CeloscanSettings> settings,
            ICeloscanClient client,
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
        public string DefiLLamaChainId => "celo";

        /// <inheritdoc />
        public string CoingeckoNativeTokenId => "celo";

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

            string tokenName = "celo";
            string? balanceWei;
            decimal usdBalance = 0;
            decimal multiplier = 1;
            var transactions = new List<CeloscanAccountNormalTransaction>();
            var internalTransactions = new List<CeloscanAccountInternalTransaction>();
            var tokenTransfers = new List<ICeloscanAccountNftTokenEvent>();
            var erc20Tokens = (await _client.GetTransactionsAsync<CeloscanAccountERC20TokenEvents, CeloscanAccountERC20TokenEvent>(request.Address).ConfigureAwait(false)).ToList();
            await Task.Delay(100, cancellationToken).ConfigureAwait(false);
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    balanceWei = (await _client.GetBalanceAsync(request.Address).ConfigureAwait(false)).Balance;
                    usdBalance =
                        (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * balanceWei?.ToCelo() ?? 0;
                    transactions = (await _client.GetTransactionsAsync<CeloscanAccountNormalTransactions, CeloscanAccountNormalTransaction>(request.Address).ConfigureAwait(false)).ToList();
                    internalTransactions = (await _client.GetTransactionsAsync<CeloscanAccountInternalTransactions, CeloscanAccountInternalTransaction>(request.Address).ConfigureAwait(false)).ToList();
                    tokenTransfers = (await _client.GetTransactionsAsync<CeloscanAccountERC721TokenEvents, CeloscanAccountERC721TokenEvent>(request.Address).ConfigureAwait(false)).Cast<ICeloscanAccountNftTokenEvent>().ToList();
                    break;
                case ScoreType.Token:
                default:
                    if (string.IsNullOrWhiteSpace(request.TokenAddress))
                    {
                        throw new CustomException("Token contract address should be set", statusCode: HttpStatusCode.BadRequest);
                    }

                    erc20Tokens = erc20Tokens.Where(t =>
                        t.ContractAddress?.Equals(request.TokenAddress, StringComparison.InvariantCultureIgnoreCase) == true).ToList();

                    balanceWei = (await _client.GetTokenBalanceAsync(request.Address, request.TokenAddress).ConfigureAwait(false)).Balance;
                    decimal.TryParse(balanceWei, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { CurrencyDecimalSeparator = "." }, out decimal balance);
                    var tokenData = await _coingeckoService.GetTokenDataAsync("celo", request.TokenAddress).ConfigureAwait(false);
                    if (tokenData != null && tokenData.DetailPlatforms.ContainsKey("celo") && !string.IsNullOrWhiteSpace(tokenData.Id))
                    {
                        tokenName = tokenData.Name ?? string.Empty;
                        int decimals = tokenData.DetailPlatforms["celo"].DecimalPlace;
                        multiplier = 1;
                        for (int i = 0; i < decimals; i++)
                        {
                            multiplier /= 10;
                        }

                        usdBalance =
                            (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{tokenData.Id}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{tokenData.Id}"].Price * balance.ToTokenValue(multiplier) ?? 0;
                    }
                    else
                    {
                        tokenName = "unknown";
                    }

                    break;
            }

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
            if ((request as IWalletTokensSwapPairsRequest)?.GetTokensSwapPairs == true
                || (request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                foreach (string? erc20TokenContractId in erc20Tokens.Select(x => x.ContractAddress).Distinct())
                {
                    await Task.Delay(250, cancellationToken).ConfigureAwait(false);
                    var tokenBalance = (await _client.GetTokenBalanceAsync(request.Address, erc20TokenContractId!).ConfigureAwait(false)).Balance?.ToBigInteger();
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((erc20TokenContractId!, $"{DefiLLamaChainId}:{erc20TokenContractId}", tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var dexTokensData = await _dexProviderService.TokensDataAsync(new TokensDataRequest
            {
                Blockchain = Chain.Celo,
                IncludeUniversalTokenLists = true,
                FromCache = true
            }).ConfigureAwait(false);

            var tokenBalances = await _defiLlamaService
                .TokensBalancesAsync(request as IWalletTokensBalancesRequest, tokensData, dexTokensData.Data).ConfigureAwait(false);

            #endregion Tokens balances

            #region Swap pairs from DEXes

            var dexTokenSwapPairs = new List<DexTokenSwapPairsData>();
            if ((request as IWalletTokensSwapPairsRequest)?.GetTokensSwapPairs == true && tokensData.Any())
            {
                var swapPairsResult = await _dexProviderService.BlockchainSwapPairsAsync(new()
                {
                    Blockchain = (Chain)ChainId,
                    First = (request as IWalletTokensSwapPairsRequest)?.FirstSwapPairs ?? 100,
                    Skip = (request as IWalletTokensSwapPairsRequest)?.Skip ?? 0,
                    FromCache = false
                }).ConfigureAwait(false);
                if (swapPairsResult.Succeeded)
                {
                    dexTokenSwapPairs.AddRange(tokensData.Select(t =>
                        DexTokenSwapPairsData.ForSwapPairs(t.TokenContractId, t.Balance, swapPairsResult.Data, dexTokensData.Data)));
                    dexTokenSwapPairs.RemoveAll(p => !p.TokenSwapPairs.Any());
                }
            }

            #endregion Swap pairs from DEXes

            var calculator = new CeloStatCalculator(
                request.Address,
                decimal.TryParse(balanceWei, out decimal wei) ? wei : 0,
                usdBalance,
                transactions,
                internalTransactions,
                tokenTransfers,
                erc20Tokens,
                snapshotVotes,
                snapshotProposals,
                tokenBalances,
                dexTokenSwapPairs,
                greysafeReportsResponse?.Reports,
                chainanalysisReportsResponse?.Identifications);
            TWalletStats? walletStats = new();
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    walletStats = calculator.GetStats() as TWalletStats;
                    break;
                case ScoreType.Token:
                    walletStats = calculator.GetTokenStats(tokenName, multiplier) as TWalletStats;
                    break;
            }

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