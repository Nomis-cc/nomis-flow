// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorerService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;

using GraphQL;
using Microsoft.Extensions.Options;
using Nomis.AptoslabsExplorer.Calculators;
using Nomis.AptoslabsExplorer.Interfaces;
using Nomis.AptoslabsExplorer.Interfaces.Extensions;
using Nomis.AptoslabsExplorer.Interfaces.Models;
using Nomis.AptoslabsExplorer.Interfaces.Requests;
using Nomis.AptoslabsExplorer.Settings;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Converters;
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
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

// ReSharper disable InconsistentNaming

namespace Nomis.AptoslabsExplorer
{
    /// <inheritdoc cref="IAptosScoringService"/>
    internal sealed class AptoslabsExplorerService :
        BlockchainDescriptor,
        IAptosScoringService,
        IHasDefiLlamaIntegration,
        ITransientService
    {
        private readonly IAptoslabsExplorerGraphQLClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IDexProviderService _dexProviderService;
        private readonly IDefiLlamaService _defiLlamaService;

        /// <summary>
        /// Initialize <see cref="AptoslabsExplorerService"/>.
        /// </summary>
        /// <param name="settings"><see cref="AptoslabsExplorerSettings"/>.</param>
        /// <param name="client"><see cref="IAptoslabsExplorerGraphQLClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="dexProviderService"><see cref="IDexProviderService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        public AptoslabsExplorerService(
            IOptions<AptoslabsExplorerSettings> settings,
            IAptoslabsExplorerGraphQLClient client,
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

        /// <inheritdoc />
        public string DefiLLamaChainId => "aptos";

        /// <inheritdoc />
        public string CoingeckoNativeTokenId => "aptos";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            var coinBalances = (await GetAptoslabsExplorerCoinBalancesAsync(new AptoslabsExplorerCoinBalancesRequest
            {
                OwnerAddress = request.Address
            }).ConfigureAwait(false)).Data;
            ulong balanceWei = coinBalances
                .FirstOrDefault(x => x.CoinType?.Equals("0x1::aptos_coin::AptosCoin", StringComparison.InvariantCultureIgnoreCase) == true)?
                .Amount ?? 0;
            decimal usdBalance =
                (await _defiLlamaService.TokensPriceAsync(new List<string?> { $"coingecko:{CoingeckoNativeTokenId}" }).ConfigureAwait(false))?.TokensPrices[$"coingecko:{CoingeckoNativeTokenId}"].Price * balanceWei.ToAptos() ?? 0;
            var coinActivities = (await GetAptoslabsExplorerCoinActivitiesAsync(new()
            {
                OwnerAddress = request.Address
            }).ConfigureAwait(false)).Data;
            var tokens = (await GetAptoslabsExplorerTokensAsync(new()
            {
                OwnerAddress = request.Address
            }).ConfigureAwait(false)).Data;
            var tokenActivities = (await GetAptoslabsExplorerTokenActivitiesAsync(new()
            {
                Address = request.Address
            }).ConfigureAwait(false)).Data;

            #region Tokens data

            var tokensData = new List<(string TokenContractId, string? TokenContractIdWithBlockchain, BigInteger? Balance)>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                var coins = coinBalances
                    .Select(x => new
                    {
                        x.CoinType,
                        x.Amount
                    })
                    .DistinctBy(x => x.CoinType);
                foreach (var coin in coins)
                {
                    await Task.Delay(250, cancellationToken).ConfigureAwait(false);
                    var tokenBalance = new BigInteger(coin.Amount);
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((coin.CoinType!, $"{DefiLLamaChainId}:{coin.CoinType}", tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var dexTokensData = await _dexProviderService.TokensDataAsync(new TokensDataRequest
            {
                Blockchain = Chain.Aptos,
                IncludeUniversalTokenLists = true,
                FromCache = true
            }).ConfigureAwait(false);

            var tokenBalances = await _defiLlamaService
                .TokensBalancesAsync(request as IWalletTokensBalancesRequest, tokensData, dexTokensData.Data).ConfigureAwait(false);

            #endregion Tokens balances

            var walletStats = new AptosStatCalculator(
                    request.Address,
                    decimal.TryParse(balanceWei.ToString(), out decimal wei) ? wei : 0,
                    usdBalance,
                    coinBalances,
                    coinActivities,
                    tokens,
                    tokenActivities,
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

        private async Task<List<TResult>> GetDataAsync<TResult>(GraphQLRequest query, string responseAlias)
        {
            var response = await _client.SendQueryAsync<JsonObject>(query).ConfigureAwait(false);
            var data = JsonSerializer.Deserialize<List<TResult>>(response.Data[responseAlias]?.ToJsonString(new()
            {
                Converters = { new BigIntegerConverter() }
            }) !) ?? new List<TResult>();

            return data;
        }

        private async Task<Result<List<AptoslabsExplorerCoinBalance>>> GetAptoslabsExplorerCoinBalancesAsync(
            AptoslabsExplorerCoinBalancesRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query CoinsData($owner_address: String, $limit: Int, $offset: Int) {
                  current_coin_balances(
                    where: {owner_address: {_eq: $owner_address}, amount: {_gt: "0"}}
                    order_by: {last_transaction_version: desc}
                    limit: $limit
                    offset: $offset
                  ) {
                    amount
                    coin_type
                    coin_info {
                      name
                      decimals
                      symbol
                    }
                  }
                }
                """,
                Variables = request
            };

            var result = new List<AptoslabsExplorerCoinBalance>();
            var data = await GetDataAsync<AptoslabsExplorerCoinBalance>(query, "current_coin_balances").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerCoinBalancesRequest
                {
                    OwnerAddress = request.OwnerAddress,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerCoinBalance>(query, "current_coin_balances").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerCoinBalance>>.SuccessAsync(result, "Coin balances received.").ConfigureAwait(false);
        }

        private async Task<Result<List<AptoslabsExplorerCoinActivity>>> GetAptoslabsExplorerCoinActivitiesAsync(
            AptoslabsExplorerCoinActivitiesRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query CoinActivity($owner_address: String, $offset: Int, $limit: Int) {
                  coin_activities(
                    where: {owner_address: {_eq: $owner_address}}
                    order_by: {transaction_version: desc}
                    offset: $offset
                    limit: $limit
                  ) {
                    activity_type
                    amount
                    coin_type
                    entry_function_id_str
                    transaction_version
                    transaction_timestamp
                    is_transaction_success
                  }
                }
                """,
                Variables = request
            };

            var result = new List<AptoslabsExplorerCoinActivity>();
            var data = await GetDataAsync<AptoslabsExplorerCoinActivity>(query, "coin_activities").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerCoinActivitiesRequest
                {
                    OwnerAddress = request.OwnerAddress,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerCoinActivity>(query, "coin_activities").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerCoinActivity>>.SuccessAsync(result, "Coin coin activities received.").ConfigureAwait(false);
        }

        private async Task<Result<List<AptoslabsExplorerToken>>> GetAptoslabsExplorerTokensAsync(
            AptoslabsExplorerTokensRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query CurrentTokens($owner_address: String, $offset: Int, $limit: Int) {
                  current_token_ownerships(
                    where: {owner_address: {_eq: $owner_address}, amount: {_gt: "0"}, table_type: {_eq: "0x3::token::TokenStore"}}
                    order_by: {last_transaction_version: desc}
                    offset: $offset
                    limit: $limit
                  ) {
                    token_data_id_hash
                    name
                    collection_name
                    property_version
                    amount
                    last_transaction_version
                  }
                }
                """,
                Variables = request
            };

            var result = new List<AptoslabsExplorerToken>();
            var data = await GetDataAsync<AptoslabsExplorerToken>(query, "current_token_ownerships").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerTokensRequest
                {
                    OwnerAddress = request.OwnerAddress,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerToken>(query, "current_token_ownerships").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerToken>>.SuccessAsync(result, "Coin tokens received.").ConfigureAwait(false);
        }

        private async Task<Result<List<AptoslabsExplorerTokenActivity>>> GetAptoslabsExplorerTokenActivitiesAsync(
            AptoslabsExplorerTokenActivitiesRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query TokenActivities($address: String, $offset: Int, $limit: Int) {
                  token_activities(
                    where: {_or: [{to_address: {_eq: $address}}, {from_address: {_eq: $address}}]}
                    order_by: {transaction_version: desc}
                    offset: $offset
                    limit: $limit
                  ) {
                    transaction_version
                    from_address
                    to_address
                    token_amount
                    transfer_type
                    token_data_id_hash
                  }
                }
                """,
                Variables = request
            };

            var result = new List<AptoslabsExplorerTokenActivity>();
            var data = await GetDataAsync<AptoslabsExplorerTokenActivity>(query, "token_activities").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerTokenActivitiesRequest
                {
                    Address = request.Address,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerTokenActivity>(query, "token_activities").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerTokenActivity>>.SuccessAsync(result, "Coin token activities received.").ConfigureAwait(false);
        }
    }
}