// ------------------------------------------------------------------------------------------------------
// <copyright file="NearblocksService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Net.Http.Json;
using System.Numerics;
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
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Domain.Scoring.Entities;
using Nomis.HapiExplorer.Interfaces;
using Nomis.HapiExplorer.Interfaces.Contracts;
using Nomis.HapiExplorer.Interfaces.Responses;
using Nomis.Nearblocks.Calculators;
using Nomis.Nearblocks.Interfaces;
using Nomis.Nearblocks.Interfaces.Models;
using Nomis.Nearblocks.Interfaces.Requests;
using Nomis.Nearblocks.Settings;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces.Models;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Wrapper;

namespace Nomis.Nearblocks
{
    /// <inheritdoc cref="INearScoringService"/>
    internal sealed class NearblocksService :
        BlockchainDescriptor,
        INearScoringService,
        IScopedService
    {
        private readonly INearblocksGraphQLClient _client;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IHapiExplorerService _hapiExplorerService;
        private readonly HttpClient _restClient;

        /// <summary>
        /// Initialize <see cref="NearblocksService"/>.
        /// </summary>
        /// <param name="settings"><see cref="NearblocksSettings"/>.</param>
        /// <param name="client"><see cref="INearblocksGraphQLClient"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="hapiExplorerService"><see cref="IHapiExplorerService"/>.</param>
        public NearblocksService(
            IOptions<NearblocksSettings> settings,
            INearblocksGraphQLClient client,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService,
            IHapiExplorerService hapiExplorerService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _hapiExplorerService = hapiExplorerService;
            _restClient = new()
            {
                BaseAddress = new(settings.Value.RestApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(settings), $"{nameof(settings.Value.RestApiBaseUrl)} does not exist."))
            };
        }

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            string? balanceString = (await GetBalanceAsync(request.Address).ConfigureAwait(false)).Balance?.Replace(",", string.Empty, StringComparison.OrdinalIgnoreCase);
            decimal.TryParse(balanceString, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." }, out decimal balance);
            decimal usdBalance = await GetUsdBalanceAsync(balance).ConfigureAwait(false);
            var transactions = (await GetNearblocksTransactionsAsync(new()
            {
                Address = request.Address,
                Limit = 1000,
                Offset = 0
            }).ConfigureAwait(false)).Data;
            var fungibleTokens = (await GetNearblocksFungibleTokensAsync(new(request.Address)).ConfigureAwait(false)).Data;
            var tokens = (await GetNearblocksNonFungibleTokenEventsAsync(new(request.Address)).ConfigureAwait(false)).Data;

            #region HAPI protocol

            HapiProxyRiskScoreResponse? hapiRiskScore = null;
            if ((request as IWalletHapiProtocolRequest)?.GetHapiProtocolData == true)
            {
                try
                {
                    hapiRiskScore = (await _hapiExplorerService.GetWalletRiskScoreAsync("near", request.Address).ConfigureAwait(false)).Data;
                }
                catch (NoDataException)
                {
                    // ignored
                }
            }

            #endregion HAPI protocol

            #region Tokens balances

            var tokenBalances = new List<TokenBalanceData>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                var balances = fungibleTokens
                    .Select(t => new TokenBalanceData(
                        new TokenPriceData
                        {
                            Confidence = 0.9M,
                            Decimals = t.Meta?.Decimals,
                            Timestamp = (ulong)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                            Price = t.Meta?.Price ?? 0,
                            Symbol = t.Meta?.Symbol
                        },
                        t.Contract!,
                        null,
                        new BigInteger(t.Amount)));
                tokenBalances.AddRange(balances);
            }

            #endregion Tokens balances

            var walletStats = new NearStatCalculator(
                    request.Address,
                    balance,
                    usdBalance,
                    transactions,
                    fungibleTokens,
                    tokens,
                    tokenBalances,
                    hapiRiskScore)
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
                }, "Get token signature: Can't get signature without HAPI Protocol adjusting score.").ConfigureAwait(false);
            if ((request as IWalletHapiProtocolRequest)?.GetHapiProtocolData == true)
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

        private async Task<List<TResult>> GetDataAsync<TResult>(GraphQLRequest query, string responseAlias)
        {
            var response = await _client.SendQueryAsync<JsonObject>(query).ConfigureAwait(false);
            var data = JsonSerializer.Deserialize<List<TResult>>(response.Data[responseAlias]?.ToJsonString(new()) !, new JsonSerializerOptions
            {
                Converters = { new BigIntegerConverter() }
            }) ?? new List<TResult>();

            return data;
        }

        private async Task<Result<List<NearblocksTransaction>>> GetNearblocksTransactionsAsync(
            GetNearblocksTransactionsRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query ($address: String, $limit: Int, $offset: Int, $order: [transactions_order_by!]) {
                  transactions(
                    limit: $limit
                    offset: $offset
                    order_by: $order
                    where: {_and: [{receipts: {_or: [{predecessor_account_id: {_eq: $address}}, {receiver_account_id: {_eq: $address}}]}}]}
                  ) {
                    ...transactionsFull
                    block {
                      ...blocks
                    }
                    receipts(order_by: {included_in_block_timestamp: asc}) {
                      ...receiptsFull
                    }
                  }
                }

                fragment blocks on blocks {
                  block_height
                  block_hash
                  block_timestamp
                  author_account_id
                }

                fragment receiptsFull on receipts {
                  execution_outcome {
                    status
                  }
                  action_receipt_actions{
                    action_kind
                    args
                  }
                  receipt_id
                  included_in_block_hash
                  included_in_block_timestamp
                  predecessor_account_id
                  receiver_account_id
                  receipt_kind
                  originated_from_transaction_hash
                }

                fragment transactionsFull on transactions {
                  transaction_hash
                  included_in_block_hash
                  block_timestamp
                  signer_account_id
                  receiver_account_id
                  status
                  receipt_conversion_tokens_burnt
                }
                """,
                Variables = request
            };

            var result = new List<NearblocksTransaction>();
            var data = await GetDataAsync<NearblocksTransaction>(query, "transactions").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new GetNearblocksTransactionsRequest
                {
                    Address = request.Address,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<NearblocksTransaction>(query, "transactions").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<NearblocksTransaction>>.SuccessAsync(result, "Transactions received.").ConfigureAwait(false);
        }

        private async Task<Result<List<NearblocksFungibleToken>>> GetNearblocksFungibleTokensAsync(
            GetNearblocksFungibleTokensRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query ($id: String, $limit: Int, $offset: Int) {
                  ft_inventory(args: {address: $id, take: $limit, skip: $offset}, where: {ft_meta: {}}) {
                    amount
                    contract
                    ft_meta {
                      ...ftMetaFull
                    }
                  }
                }

                fragment ftMetaFull on ft_meta {
                  contract
                  name
                  decimals
                  symbol
                  icon
                  spec
                  reference
                  reference_hash
                  price
                  description
                  twitter
                  facebook
                  telegram
                  reddit
                  website
                }
                """,
                Variables = request
            };

            var result = new List<NearblocksFungibleToken>();
            var data = await GetDataAsync<NearblocksFungibleToken>(query, "ft_inventory").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new GetNearblocksFungibleTokensRequest(request.Id, request.Limit, request.Offset + request.Limit);
                query.Variables = request;
                data = await GetDataAsync<NearblocksFungibleToken>(query, "ft_inventory").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<NearblocksFungibleToken>>.SuccessAsync(result, "Fungible tokens received.").ConfigureAwait(false);
        }

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1213 // Remove unused member declaration.
        private async Task<Result<List<NearblocksFungibleTokenEvent>>> GetNearblocksFungibleTokenEventsAsync(
#pragma warning restore RCS1213 // Remove unused member declaration.
#pragma warning restore IDE0051 // Remove unused private members
            GetNearblocksFungibleTokenEventsRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query ($limit: Int, $offset: Int, $id: String) {
                  assets__fungible_token_events(
                    limit: $limit
                    offset: $offset
                    order_by: {emitted_at_block_timestamp: desc}
                    where: {_or: [{emitted_by_contract_account_id: {_eq: $id}}, {token_new_owner_account_id: {_eq: $id}}, {token_old_owner_account_id: {_eq: $id}}]}
                  ) {
                    amount
                    emitted_for_receipt_id
                    emitted_at_block_timestamp
                    emitted_by_contract_account_id
                    event_kind
                    token_old_owner_account_id
                    token_new_owner_account_id
                    receipt {
                      execution_outcome {
                        status
                      }
                      action_receipt_actions{
                        action_kind
                        args
                      }
                      transaction {
                        transaction_hash
                        included_in_block_hash
                        block_timestamp
                        signer_account_id
                        receiver_account_id
                        status
                        block {
                          block_height
                          block_hash
                          block_timestamp
                          author_account_id
                        }
                      }
                    }
                    ft_meta {
                      ...ftMetaFull
                    }
                  }
                }

                fragment ftMetaFull on ft_meta {
                  contract
                  name
                  decimals
                  symbol
                  icon
                  spec
                  reference
                  reference_hash
                  price
                  description
                  twitter
                  facebook
                  telegram
                  reddit
                  website
                }
                """,
                Variables = request
            };

            var result = new List<NearblocksFungibleTokenEvent>();
            var data = await GetDataAsync<NearblocksFungibleTokenEvent>(query, "assets__fungible_token_events").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new GetNearblocksFungibleTokenEventsRequest(request.Id, request.Limit, request.Offset + request.Limit);
                query.Variables = request;
                data = await GetDataAsync<NearblocksFungibleTokenEvent>(query, "assets__fungible_token_events").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<NearblocksFungibleTokenEvent>>.SuccessAsync(result, "Fungible token events received.").ConfigureAwait(false);
        }

        private async Task<Result<List<NearblocksNonFungibleTokenEvent>>> GetNearblocksNonFungibleTokenEventsAsync(
            GetNearblocksNonFungibleTokenEventsRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query ($limit: Int, $offset: Int, $id: String) {
                  assets__non_fungible_token_events(
                    limit: $limit
                    offset: $offset
                    order_by: {emitted_at_block_timestamp: desc}
                    where: {_or: [{emitted_by_contract_account_id: {_eq: $id}}, {token_new_owner_account_id: {_eq: $id}}, {token_old_owner_account_id: {_eq: $id}}]}
                  ) {
                    emitted_for_receipt_id
                    emitted_at_block_timestamp
                    emitted_by_contract_account_id
                    token_id
                    event_kind
                    token_old_owner_account_id
                    token_new_owner_account_id
                    receipt {
                      execution_outcome {
                        status
                      }
                      action_receipt_actions{
                        action_kind
                        args
                      }
                      transaction {
                        transaction_hash
                        included_in_block_hash
                        block_timestamp
                        signer_account_id
                        receiver_account_id
                        status
                        block {
                          block_height
                          block_hash
                          block_timestamp
                          author_account_id
                        }
                      }
                    }
                    nft_meta {
                      ...nftMetaFull
                    }
                  }
                }

                fragment nftMetaFull on nft_meta {
                  contract
                  name
                  symbol
                  spec
                  base_uri
                  reference
                  reference_hash
                  description
                  twitter
                  facebook
                  telegram
                  reddit
                  website
                }
                """,
                Variables = request
            };

            var result = new List<NearblocksNonFungibleTokenEvent>();
            var data = await GetDataAsync<NearblocksNonFungibleTokenEvent>(query, "assets__non_fungible_token_events").ConfigureAwait(false);
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new GetNearblocksNonFungibleTokenEventsRequest(request.Id, request.Limit, request.Offset + request.Limit);
                query.Variables = request;
                data = await GetDataAsync<NearblocksNonFungibleTokenEvent>(query, "assets__non_fungible_token_events").ConfigureAwait(false);
                result.AddRange(data);
            }

            return await Result<List<NearblocksNonFungibleTokenEvent>>.SuccessAsync(result, "Non fungible token events received.").ConfigureAwait(false);
        }

        private async Task<NearblocksAccount> GetBalanceAsync(string address)
        {
            string request =
                $"/api/account/balance?address={address}";

            var response = await _restClient.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NearblocksAccount>()
.ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        private async Task<decimal> GetUsdBalanceAsync(decimal balance)
        {
            var response = await _restClient.GetAsync("/api/near-price").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<NearblocksNearPrice>()
.ConfigureAwait(false) ?? throw new CustomException("Can't get USD balance.");
            return balance * data.Usd;
        }
    }
}