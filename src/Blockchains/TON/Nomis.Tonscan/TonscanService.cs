// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Nodes;

using GraphQL;
using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Domain.Scoring.Entities;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Tonscan.Calculators;
using Nomis.Tonscan.Interfaces;
using Nomis.Tonscan.Interfaces.Extensions;
using Nomis.Tonscan.Models;
using Nomis.Tonscan.Requests;
using Nomis.Tonscan.Responses;
using Nomis.Tonscan.Settings;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.Tonscan
{
    /// <inheritdoc cref="ITonScoringService"/>
    internal sealed class TonscanService :
        BlockchainDescriptor,
        ITonScoringService,
        ITransientService
    {
        private readonly ITonscanClient _client;
        private readonly IGetGemsGraphQLClient _getGemsGraphQlClient;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IDefiLlamaService _defiLlamaService;

        /// <summary>
        /// Initialize <see cref="TonscanService"/>.
        /// </summary>
        /// <param name="settings"><see cref="TonscanSettings"/>.</param>
        /// <param name="client"><see cref="ITonscanClient"/>.</param>
        /// <param name="getGemsGraphQlClient"><see cref="IGetGemsGraphQLClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        public TonscanService(
            IOptions<TonscanSettings> settings,
            ITonscanClient client,
            IGetGemsGraphQLClient getGemsGraphQlClient,
            ICoingeckoService coingeckoService,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService,
            IDefiLlamaService defiLlamaService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _getGemsGraphQlClient = getGemsGraphQlClient;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _defiLlamaService = defiLlamaService;
        }

        /// <summary>
        /// Coingecko native token id.
        /// </summary>
        public string CoingeckoNativeTokenId => "the-open-network";

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
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * balanceWei.ToTon() ?? 0;
            var transactions = (await _client.GetTransactionsAsync(request.Address).ConfigureAwait(false)).ToList();
            var assets = (await _client.GetAssetsAsync(request.Address).ConfigureAwait(false)).Balances;
            var nftAssets = (await GetGetGemsNftItemsByOwnerAsync(new GetGetGemsNftItemsByOwnerRequest(request.Address)).ConfigureAwait(false)).Data;

            #region Tokens balances

            var tokenBalances = new List<TokenBalanceData>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                var balances = assets
                    .Select(t => new TokenBalanceData(
                        new TokenPriceData
                        {
                            Confidence = 0,
                            Decimals = t.Metadata?.Decimals,
                            Timestamp = (ulong)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                            Price = 0,
                            Symbol = t.Metadata?.Symbol
                        },
                        t.JettonAddress!,
                        null,
                        t.Balance?.ToBigInteger()));
                tokenBalances.AddRange(balances);
            }

            #endregion Tokens balances

            var walletStats = new TonStatCalculator(
                    request.Address,
                    (decimal)balanceWei,
                    usdBalance,
                    transactions,
                    assets,
                    nftAssets,
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

        private async Task<TResult> GetDataAsync<TResult>(GraphQLRequest query, string responseAlias)
            where TResult : class, new()
        {
            var response = await _getGemsGraphQlClient.SendQueryAsync<JsonObject>(query).ConfigureAwait(false);
            var data = JsonSerializer.Deserialize<TResult>(response.Data[responseAlias]?.ToJsonString(new()) !, new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }) ?? new TResult();

            return data;
        }

        private async Task<Result<IList<GetGemsNftItem>>> GetGetGemsNftItemsByOwnerAsync(
            GetGetGemsNftItemsByOwnerRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query NftItemConnection($ownerAddress: String!, $first: Int!, $after: String) {
                  nftItemsByOwner(ownerAddress: $ownerAddress, first: $first, after: $after) {
                    cursor
                    items {
                      id
                      name
                      address
                      kind
                      collection {
                        address
                        name
                        isVerified
                      }
                      sale {
                        ... on NftSaleFixPrice {
                          fullPrice
                          nftOwnerAddress
                          address
                        }
                      }
                    }
                  }
                }
                """,
                Variables = request
            };

            var data = await GetDataAsync<GetGemsNftItemsResponse>(query, "nftItemsByOwner").ConfigureAwait(false);
            return await Result<IList<GetGemsNftItem>>.SuccessAsync(data.Items, "Non fungible token assets received.").ConfigureAwait(false);
        }
    }
}