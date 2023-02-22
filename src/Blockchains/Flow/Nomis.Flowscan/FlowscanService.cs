// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

using GraphQL;
using Microsoft.Extensions.Options;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Domain.Scoring.Entities;
using Nomis.Flowscan.Calculators;
using Nomis.Flowscan.Interfaces;
using Nomis.Flowscan.Interfaces.Extensions;
using Nomis.Flowscan.Interfaces.Models;
using Nomis.Flowscan.Interfaces.Requests;
using Nomis.Flowscan.Settings;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Extensions;
using Nomis.Utils.Wrapper;

namespace Nomis.Flowscan
{
    /// <inheritdoc cref="IFlowScoringService"/>
    internal sealed class FlowscanService :
        BlockchainDescriptor,
        IFlowScoringService,
        IScopedService
    {
        private readonly IFlowscanGraphQLClient _client;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;

        /// <summary>
        /// Initialize <see cref="FlowscanService"/>.
        /// </summary>
        /// <param name="settings"><see cref="FlowscanSettings"/>.</param>
        /// <param name="client"><see cref="IFlowscanGraphQLClient"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        public FlowscanService(
            IOptions<FlowscanSettings> settings,
            IFlowscanGraphQLClient client,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
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
            var account = await GetAccountAsync(new(request.Address)).ConfigureAwait(false);
            if (account == null)
            {
                return await Result<TWalletScore>.FailAsync("Failed to get account data for this wallet.").ConfigureAwait(false);
            }

            string? balanceString = account
                .TokenBalances?
                .Edges
                .FirstOrDefault(e => e.Node?.Amount?.Token?.Ticker?.Equals("FLOW", StringComparison.OrdinalIgnoreCase) == true)?
                .Node?.Amount?.Value;
            decimal.TryParse(balanceString, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." }, out decimal balance);
            decimal usdBalance = account
                .TokenBalances?
                .Edges
                .FirstOrDefault(e => e.Node?.Amount?.Token?.Ticker?.Equals("FLOW", StringComparison.OrdinalIgnoreCase) == true)?
                .Node?.Amount?.UsdValue ?? 0;

            var transactions = (await GetFlowscanTransactionsAsync(new GetFlowscanTransactionsRequest(request.Address)).ConfigureAwait(false)).Data;
            var tokenTransfers = (await GetFlowscanTransfersAsync(new GetFlowscanTransfersRequest(request.Address)).ConfigureAwait(false)).Data;
            var nftTokenTransfers = (await GetFlowscanNftTransfersAsync(new GetFlowscanTransfersRequest(request.Address)).ConfigureAwait(false)).Data;

            #region Tokens balances (DefiLlama)

            var tokenBalances = new List<TokenBalanceData>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                foreach (var tokenBalanceData in account.TokenBalances?.Edges ?? new List<FlowscanAccountBalanceEdge>())
                {
                    var tokenPriceData = new TokenPriceData
                    {
                        Confidence = 0.9M,
                        Decimals = 8,
                        Price = (tokenBalanceData.Node?.Amount?.UsdValue ?? 0) / (tokenBalanceData.Node?.Amount?.Value?.ToFlow() ?? 1),
                        Timestamp = (ulong)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Symbol = tokenBalanceData.Node?.Amount?.Token?.Ticker ?? tokenBalanceData.Node?.Amount?.Token?.Id
                    };
                    tokenBalances.Add(new TokenBalanceData(tokenPriceData, $"{tokenBalanceData.Node?.Amount?.Token?.Id}", $"flow:{tokenBalanceData.Node?.Amount?.Token?.Id}", tokenBalanceData.Node?.Amount?.Value?.ToBigInteger()));
                }
            }

            #endregion Tokens balances

            var walletStats = new FlowStatCalculator(
                    request.Address,
                    balance,
                    usdBalance,
                    account,
                    transactions,
                    tokenTransfers,
                    nftTokenTransfers,
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

        private async Task<FlowscanAccount?> GetAccountAsync(
            GetFlowscanTokensRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query AccountHoldings ($accountId: ID!) {
                  account(id: $accountId) {
                    contracts{
                      id
                    }
                    address
                    tokenBalances {
                      edges {
                        node {
                          amount {
                            token {
                              id
                              name
                              ticker
                            }
                            value
                            usdValue
                          }
                        }
                      }
                    }
                  }
                }
                """,
                Variables = request
            };

            var response = await _client.SendQueryAsync<JsonObject>(query).ConfigureAwait(false);
            return response.Data["account"] == null
                ? null
                : JsonSerializer.Deserialize<FlowscanAccount>(response.Data["account"]!.ToJsonString());
        }

        private async Task<TResult> GetDataAsync<TResult>(GraphQLRequest query, params string[] responseAliases)
        {
            var responseAliasList = responseAliases.ToList();
            var response = await _client.SendQueryAsync<JsonObject>(query).ConfigureAwait(false);
            var result = response.Data[responseAliasList.First()];
            responseAliasList.RemoveAt(0);
            foreach (string responseAlias in responseAliasList)
            {
                result = result![responseAlias];
            }

            var data = JsonSerializer.Deserialize<TResult>(result!.ToJsonString()) !;

            return data;
        }

        private async Task<Result<List<FlowscanTransaction>>> GetFlowscanTransactionsAsync(
            GetFlowscanTransactionsRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query AccountTransactions ($accountId: ID!, $after: ID) {
                  account(id: $accountId) {
                    address
                    transactions (
                      first: 5
                      ordering: Descending
                      after: $after
                    ) {
                      pageInfo {
                        hasNextPage
                        endCursor
                      }
                      edges {
                        node {
                          hash
                          time
                          status
                        }
                      }
                    }
                  }
                }
                """,
                Variables = request
            };

            var result = new List<FlowscanTransaction>();
            var data = await GetDataAsync<FlowscanAccountTransactions>(query, "account", "transactions").ConfigureAwait(false);
            result.AddRange(data.Edges.Select(e => e.Transaction) !);
            while (data.PageInfo?.HasNextPage == true)
            {
                request = new GetFlowscanTransactionsRequest(request.AccountId!, data.PageInfo?.EndCursor);
                query.Variables = request;
                data = await GetDataAsync<FlowscanAccountTransactions>(query, "account", "transactions").ConfigureAwait(false);
                result.AddRange(data.Edges.Select(e => e.Transaction) !);
            }

            return await Result<List<FlowscanTransaction>>.SuccessAsync(result, "Transactions received.").ConfigureAwait(false);
        }

        private async Task<Result<List<FlowscanAccountTokenTransferEdgeNode>>> GetFlowscanTransfersAsync(
            GetFlowscanTransfersRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query AccountTransfers($accountId: ID!, $after: ID) {
                  account(id: $accountId) {
                    tokenTransfers (
                      first: 50
                      ordering: Descending
                      after: $after
                    ) {
                      pageInfo {
                        hasNextPage
                        endCursor
                      }
                      edges {
                        node {
                          transaction {
                            hash
                            time
                            status
                          }
                          type
                          amount {
                            token {
                              id
                              name
                              ticker
                              description
                            }
                            value
                            usdValue
                          }
                          counterparty {
                            address
                          }
                          counterpartiesCount
                        }
                      }
                    }
                  }
                }
                """,
                Variables = request
            };

            var result = new List<FlowscanAccountTokenTransferEdgeNode>();
            var data = await GetDataAsync<FlowscanAccountTokenTransfers>(query, "account", "tokenTransfers").ConfigureAwait(false);
            result.AddRange(data.Edges.Select(e => e.Node) !);
            while (data.PageInfo?.HasNextPage == true)
            {
                request = new GetFlowscanTransfersRequest(request.AccountId!, data.PageInfo?.EndCursor);
                query.Variables = request;
                data = await GetDataAsync<FlowscanAccountTokenTransfers>(query, "account", "tokenTransfers").ConfigureAwait(false);
                result.AddRange(data.Edges.Select(e => e.Node) !);
            }

            return await Result<List<FlowscanAccountTokenTransferEdgeNode>>.SuccessAsync(result, "Token transfers received.").ConfigureAwait(false);
        }

        private async Task<Result<List<FlowscanAccountNftTransferEdgeNode>>> GetFlowscanNftTransfersAsync(
            GetFlowscanTransfersRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = """
                query AccountTransfers($accountId: ID!, $after: ID) {
                  account(id: $accountId) {
                    nftTransfers (
                      first: 50
                      ordering: Descending
                      after: $after
                    ) {
                      pageInfo {
                        hasNextPage
                        endCursor
                      }
                      edges {
                        node {
                          transaction {
                            hash
                            time
                            status
                          }
                          nft {
                            contract {
                              id
                              address
                            }
                            nftId
                          }
                          from {
                            address
                          }
                          to {
                            address
                          }
                        }
                      }
                    }
                  }
                }
                """,
                Variables = request
            };

            var result = new List<FlowscanAccountNftTransferEdgeNode>();
            var data = await GetDataAsync<FlowscanAccountNftTransfers>(query, "account", "nftTransfers").ConfigureAwait(false);
            result.AddRange(data.Edges.Select(e => e.Node) !);
            while (data.PageInfo?.HasNextPage == true)
            {
                request = new GetFlowscanTransfersRequest(request.AccountId!, data.PageInfo?.EndCursor);
                query.Variables = request;
                data = await GetDataAsync<FlowscanAccountNftTransfers>(query, "account", "nftTransfers").ConfigureAwait(false);
                result.AddRange(data.Edges.Select(e => e.Node) !);
            }

            return await Result<List<FlowscanAccountNftTransferEdgeNode>>.SuccessAsync(result, "NFT token transfers received.").ConfigureAwait(false);
        }
    }
}