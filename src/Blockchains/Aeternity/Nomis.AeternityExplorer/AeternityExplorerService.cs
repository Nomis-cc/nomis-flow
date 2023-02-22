// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorerService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.AeternityExplorer.Calculators;
using Nomis.AeternityExplorer.Interfaces;
using Nomis.AeternityExplorer.Interfaces.Extensions;
using Nomis.AeternityExplorer.Interfaces.Models;
using Nomis.AeternityExplorer.Settings;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.Domain.Scoring.Entities;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

namespace Nomis.AeternityExplorer
{
    /// <inheritdoc cref="IAeternityScoringService"/>
    internal sealed class AeternityExplorerService :
        BlockchainDescriptor,
        IAeternityScoringService,
        ITransientService
    {
        private readonly IAeternityExplorerClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IDefiLlamaService _defiLlamaService;

        /// <summary>
        /// Initialize <see cref="AeternityExplorerService"/>.
        /// </summary>
        /// <param name="settings"><see cref="AeternityExplorerSettings"/>.</param>
        /// <param name="client"><see cref="IAeternityExplorerClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        public AeternityExplorerService(
            IOptions<AeternityExplorerSettings> settings,
            IAeternityExplorerClient client,
            ICoingeckoService coingeckoService,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService,
            IDefiLlamaService defiLlamaService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _defiLlamaService = defiLlamaService;
        }

        /// <summary>
        /// Coingecko native token id.
        /// </summary>
        public string CoingeckoNativeTokenId => "aeternity";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            var balanceWei = (await _client.GetBalanceAsync(request.Address).ConfigureAwait(false)).Balance;
            decimal usdBalance =
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * balanceWei.ToAeternity() ?? 0;
            var transactions = (await _client.GetTransactionsAsync<AeternityExplorerAccountNormalTransactions, AeternityExplorerAccountNormalTransaction>(request.Address).ConfigureAwait(false)).ToList();
            var internalTransactions = (await _client.GetTransactionsAsync<AeternityExplorerAccountInternalTransactions, AeternityExplorerAccountInternalTransaction>(request.Address).ConfigureAwait(false)).ToList();
            var aex9FromTokens = (await _client.GetTransactionsAsync<AeternityExplorerAccountAEX9TokenEvents, AeternityExplorerAccountAEX9TokenEvent>(request.Address, true).ConfigureAwait(false)).ToList();
            var aex9ToTokens = (await _client.GetTransactionsAsync<AeternityExplorerAccountAEX9TokenEvents, AeternityExplorerAccountAEX9TokenEvent>(request.Address, false).ConfigureAwait(false)).ToList();
            var aex9Tokens = aex9FromTokens.Union(aex9ToTokens).ToList();
            var aex141FromTokens = (await _client.GetTransactionsAsync<AeternityExplorerAccountAEX141TokenEvents, AeternityExplorerAccountAEX141TokenEvent>(request.Address, true).ConfigureAwait(false)).ToList();
            var aex141ToTokens = (await _client.GetTransactionsAsync<AeternityExplorerAccountAEX141TokenEvents, AeternityExplorerAccountAEX141TokenEvent>(request.Address, false).ConfigureAwait(false)).ToList();
            var aex141Tokens = aex141FromTokens.Union(aex141ToTokens).ToList();

            var walletStats = new AeternityStatCalculator(
                    request.Address,
                    (decimal)balanceWei,
                    usdBalance,
                    transactions,
                    internalTransactions,
                    aex141Tokens,
                    aex9Tokens)
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