// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Flowscan.Interfaces.Extensions;
using Nomis.Flowscan.Interfaces.Models;

namespace Nomis.Flowscan.Calculators
{
    /// <summary>
    /// Flow wallet stats calculator.
    /// </summary>
    internal sealed class FlowStatCalculator :
        IStatCalculator<FlowWalletStats, FlowTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly FlowscanAccount _account;
        private readonly IEnumerable<FlowscanTransaction> _transactions;
        private readonly IEnumerable<FlowscanAccountTokenTransferEdgeNode> _transfers;
        private readonly IEnumerable<FlowscanAccountNftTransferEdgeNode> _nftTransfers;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;

        public FlowStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            FlowscanAccount account,
            IEnumerable<FlowscanTransaction> transactions,
            IEnumerable<FlowscanAccountTokenTransferEdgeNode> transfers,
            IEnumerable<FlowscanAccountNftTransferEdgeNode> nftTransfers,
            IEnumerable<TokenBalanceData>? tokenBalances)
        {
            _address = address;
            _balance = balance;
            _usdBalance = usdBalance;
            _transfers = transfers;
            _account = account;
            _transactions = transactions;
            _nftTransfers = nftTransfers;
            _tokenBalances = tokenBalances;
        }

        public FlowWalletStats GetStats()
        {
            if (!_transfers.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transfers.Select(x => x.Transaction!.Time)).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _nftTransfers
                .Where(x => x.From?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true)
                .ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokens.Select(x => x.Transaction?.Hash!), _transfers.Select(x => (x.Transaction?.Hash!, BigInteger.Parse(x.Amount?.Value!))));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _nftTransfers
                .Where(t => t.To?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(t.GetTokenUid()))
                .ToList();
            var buySum = IStatCalculator
                .GetTokensSum(buyTokens.Select(x => x.Transaction?.Hash!), _transfers.Select(x => (x.Transaction?.Hash!, BigInteger.Parse(x.Amount?.Value!))));

            var buyNotSoldTokens = _nftTransfers
                .Where(t => t.To?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(t.GetTokenUid()))
                .ToList();
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(buyNotSoldTokens.Select(x => x.Transaction?.Hash!), _transfers.Select(x => (x.Transaction?.Hash!, BigInteger.Parse(x.Amount?.Value!))));

            int holdingTokens = _nftTransfers.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;

            int contractsCreated = _account.Contracts.Count;
            int totalTokens = _account.TokenBalances?.Edges.Count ?? 0;

            var turnoverIntervalsDataList = _transfers
                .Select(x => new TurnoverIntervalsData(
                x.Transaction!.Time,
                new BigInteger(decimal.Parse(x.Amount!.Value ?? "0")),
                x.Counterparty?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) != true));
            var turnoverIntervals = IStatCalculator<FlowTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transfers.Min(x => x.Transaction!.Time)).ToList();

            return new()
            {
                NativeBalance = _balance.ToFlow(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transfers.Select(x => x.Transaction!.Time)),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transfers.Count(t => t.Transaction?.Status?.Equals("Error", StringComparison.InvariantCultureIgnoreCase) == true),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = turnoverIntervals.Sum(x => Math.Abs((decimal)x.AmountSum)).ToFlow(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<FlowTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<FlowTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transfers.Count(x => x.Transaction!.Time > monthAgo),
                LastYearTransactions = _transfers.Count(x => x.Transaction!.Time > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transfers.OrderBy(x => x.Transaction?.Time).Last().Transaction!.Time).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToFlow(),
                NftWorth = nftWorth.ToFlow(),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens,
                TokenBalances = _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice)
            };
        }
    }
}