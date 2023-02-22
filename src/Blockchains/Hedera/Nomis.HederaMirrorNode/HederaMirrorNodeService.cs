// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeService.cs" company="Nomis">
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
using Nomis.Dex.Abstractions.Enums;
using Nomis.DexProviderService.Interfaces;
using Nomis.DexProviderService.Interfaces.Extensions;
using Nomis.DexProviderService.Interfaces.Requests;
using Nomis.Domain.Scoring.Entities;
using Nomis.HederaMirrorNode.Calculators;
using Nomis.HederaMirrorNode.Interfaces;
using Nomis.HederaMirrorNode.Interfaces.Extensions;
using Nomis.HederaMirrorNode.Interfaces.Models;
using Nomis.HederaMirrorNode.Settings;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

namespace Nomis.HederaMirrorNode
{
    /// <inheritdoc cref="IHederaScoringService"/>
    internal sealed class HederaMirrorNodeService :
        BlockchainDescriptor,
        IHederaScoringService,
        ITransientService
    {
        private readonly IHederaMirrorNodeClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IDefiLlamaService _defiLlamaService;

        /// <summary>
        /// Initialize <see cref="HederaMirrorNodeService"/>.
        /// </summary>
        /// <param name="settings"><see cref="HederaMirrorNodeSettings"/>.</param>
        /// <param name="client"><see cref="IHederaMirrorNodeClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        public HederaMirrorNodeService(
            IOptions<HederaMirrorNodeSettings> settings,
            IHederaMirrorNodeClient client,
            ICoingeckoService coingeckoService,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService,
            IDexProviderService dexProviderService,
            IDefiLlamaService defiLlamaService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _dexProviderService = dexProviderService;
            _defiLlamaService = defiLlamaService;
        }

        /// <summary>
        /// Coingecko native token id.
        /// </summary>
        public string CoingeckoNativeTokenId => "hedera-hashgraph";

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
            decimal usdBalance =
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * (accountData.Balance?.Balance ?? 0).ToHbars() ?? 0;
            var transactions = accountData.Transactions;
            var internalTransactions = accountData.Transactions.SelectMany(x => x.Transfers);
            var nfts = await _client.GetNftsAsync(request.Address).ConfigureAwait(false);
            var nftTransactions = new List<HederaMirrorNodeNftTransactionData>();
            foreach (var nft in nfts.Nfts)
            {
                nftTransactions.AddRange((await _client.GetNftTransactionsAsync(nft.TokenId!, nft.SerialNumber).ConfigureAwait(false)).Transactions);
            }

            #region Tokens data

            var tokensData = new List<(string TokenContractId, string? TokenContractIdWithBlockchain, BigInteger? Balance)>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                foreach (var token in accountData.Balance!.Tokens.DistinctBy(t => t.TokenId))
                {
                    var tokenBalance = token.Balance;
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((token.TokenId!, null, tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var dexTokensData = await _dexProviderService.TokensDataAsync(new TokensDataRequest
            {
                Blockchain = Chain.Hedera,
                IncludeUniversalTokenLists = true,
                FromCache = true
            }).ConfigureAwait(false);

            var tokenBalances = await _defiLlamaService
                .TokensBalancesAsync(request as IWalletTokensBalancesRequest, tokensData, dexTokensData.Data).ConfigureAwait(false);

            #endregion Tokens balances

            var walletStats = new HederaStatCalculator(
                    request.Address,
                    accountData,
                    usdBalance,
                    transactions,
                    internalTransactions,
                    nfts.Nfts,
                    nftTransactions,
                    tokenBalances)
                .GetStats() as TWalletStats;

            double score = walletStats!.GetScore<TWalletStats, TTransactionIntervalData>();
            var scoringData = new ScoringData(request.Address, request.Address, ChainId, score, JsonSerializer.Serialize(walletStats));
            await _scoringService.SaveScoringDataToDatabaseAsync(scoringData, cancellationToken).ConfigureAwait(false);

            // getting signature
            ushort mintedScore = (ushort)(score * 10000);
            var signatureResult = _soulboundTokenService.GetSoulboundTokenSignature(new()
            {
                Score = mintedScore,
                ScoreType = request.ScoreType,
                To = request.Address,
                Nonce = request.Nonce,
                ChainId = ChainId,
                ContractAddress = SBTContractAddresses?.ContainsKey(request.ScoreType) == true ? SBTContractAddresses?[request.ScoreType] : null,
                Deadline = request.Deadline
            });

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