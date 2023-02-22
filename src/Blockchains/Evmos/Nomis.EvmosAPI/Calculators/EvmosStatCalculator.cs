// ------------------------------------------------------------------------------------------------------
// <copyright file="EvmosStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Chainanalysis.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.EvmosAPI.Interfaces.Extensions;
using Nomis.EvmosAPI.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.Snapshot.Interfaces.Models;
using Nomis.Utils.Extensions;

namespace Nomis.EvmosAPI.Calculators
{
    /// <summary>
    /// Evmos wallet stats calculator.
    /// </summary>
    internal sealed class EvmosStatCalculator :
        IStatCalculator<EvmosWalletStats, EvmosTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<EvmosAPIAccountNormalTransaction> _transactions;
        private readonly IEnumerable<EvmosAPIAccountCommonTokenEvent> _tokenList;
        private readonly IEnumerable<EvmosAPIAccountERC20TokenEvent> _erc20TokenTransfers;
        private readonly IEnumerable<SnapshotVote>? _snapshotVotes;
        private readonly IEnumerable<SnapshotProposal>? _snapshotProposals;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;
        private readonly IEnumerable<GreysafeReport>? _greysafeReports;
        private readonly IEnumerable<ChainanalysisReport>? _chainanalysisReports;

        public EvmosStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<EvmosAPIAccountNormalTransaction> transactions,
            IEnumerable<EvmosAPIAccountCommonTokenEvent> tokenList,
            IEnumerable<EvmosAPIAccountERC20TokenEvent> erc20TokenTransfers,
            IEnumerable<SnapshotVote>? snapshotVotes,
            IEnumerable<SnapshotProposal>? snapshotProposals,
            IEnumerable<TokenBalanceData>? tokenBalances,
            IEnumerable<GreysafeReport>? greysafeReports,
            IEnumerable<ChainanalysisReport>? chainanalysisReports)
        {
            _address = address;
            _balance = balance;
            _usdBalance = usdBalance;
            _transactions = transactions;
            _tokenList = tokenList;
            _erc20TokenTransfers = erc20TokenTransfers;
            _snapshotVotes = snapshotVotes;
            _snapshotProposals = snapshotProposals;
            _tokenBalances = tokenBalances;
            _greysafeReports = greysafeReports;
            _chainanalysisReports = chainanalysisReports;
        }

        public EvmosWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.TimeStamp!.ToDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            // var soldTokens = _tokenTransfers.Where(x => x.From?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            // var soldSum = GetTokensSum(soldTokens);

            // var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            // var buyTokens = _tokenTransfers.Where(x => x.To?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            // var buySum = GetTokensSum(buyTokens);

            // var buyNotSoldTokens = _tokenTransfers.Where(x => x.To?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.GetTokenUid()));
            // var buyNotSoldSum = GetTokensSum(buyNotSoldTokens);

            int holdingTokens = _tokenList
                .Count(x => x.Type?.Equals("ERC-721", StringComparison.InvariantCultureIgnoreCase) == true ||
                            x.Type?.Equals("ERC-1155", StringComparison.InvariantCultureIgnoreCase) == true);

            // var nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            int contractsCreated = _transactions.Count(x => !string.IsNullOrWhiteSpace(x.ContractAddress));
            var totalTokens = _tokenList
                .Where(x => x.Type?.Equals("ERC-20", StringComparison.InvariantCultureIgnoreCase) == true)
                .Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.TimeStamp!.ToDateTime(),
                    BigInteger.TryParse(x.Value, out var value) ? value : 0,
                    x.From?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<EvmosTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.TimeStamp!.ToDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance.ToEvmos(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.TimeStamp!.ToDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => string.Equals(t.IsError, "1", StringComparison.OrdinalIgnoreCase)),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => decimal.TryParse(x.Value, out decimal value) ? value.ToEvmos() : 0),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<EvmosTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<EvmosTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.TimeStamp!.ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.TimeStamp!.ToDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.TimeStamp).Last().TimeStamp!.ToDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,

                // NftTrading = (soldSum - buySum).ToEvmos(),
                // NftWorth = nftWorth.ToEvmos(),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens.Count(),
                SnapshotVotes = IStatCalculator.GetSnapshotProtocolVotesData(_snapshotVotes),
                SnapshotProposals = IStatCalculator.GetSnapshotProtocolProposalsData(_snapshotProposals),
                TokenBalances = _tokenBalances?.Any() == true ? _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice) : null,
                GreysafeReports = _greysafeReports?.Any() == true ? _greysafeReports : null,
                ChainanalysisReports = _chainanalysisReports?.Any() == true ? _chainanalysisReports : null
            };
        }
    }
}