// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Flowscan.Interfaces.Extensions;
using Nomis.Flowscan.Interfaces.Models;
using Nomis.Utils.Contracts;
using Nomis.Utils.Contracts.Calculators;

namespace Nomis.Flowscan.Calculators
{
    /// <summary>
    /// Flow wallet stats calculator.
    /// </summary>
    internal sealed class FlowStatCalculator :
        IWalletCommonStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>,
        IWalletNativeBalanceStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>,
        IWalletTokenBalancesStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>,
        IWalletTransactionStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>,
        IWalletTokenStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>,
        IWalletContractStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>,
        IWalletNftStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>
    {
        private readonly string _address;
        private readonly FlowscanAccount _account;
        private readonly IEnumerable<FlowscanTransaction> _transactions;
        private readonly IEnumerable<FlowscanAccountTokenTransferEdgeNode> _transfers;
        private readonly IEnumerable<FlowscanAccountNftTransferEdgeNode> _nftTransfers;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;

        /// <inheritdoc />
        public int WalletAge => _transactions.Any()
            ? IWalletStatsCalculator.GetWalletAge(_transfers.Select(x => x.Transaction!.Time))
            : 1;

        /// <inheritdoc />
        public IList<FlowTransactionIntervalData> TurnoverIntervals
        {
            get
            {
                var turnoverIntervalsDataList = _transfers
                    .Select(x => new TurnoverIntervalsData(
                        x.Transaction!.Time,
                        new BigInteger(decimal.Parse(x.Amount!.Value ?? "0")),
                        x.Counterparty?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) != true));
                return IWalletStatsCalculator<FlowTransactionIntervalData>
                    .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Any() ? _transfers.Min(x => x.Transaction!.Time) : DateTime.MinValue).ToList();
            }
        }

        /// <inheritdoc />
        public decimal NativeBalance { get; }

        /// <inheritdoc />
        public decimal NativeBalanceUSD { get; }

        /// <inheritdoc />
        public decimal BalanceChangeInLastMonth =>
            IWalletStatsCalculator<FlowTransactionIntervalData>.GetBalanceChangeInLastMonth(TurnoverIntervals);

        /// <inheritdoc />
        public decimal BalanceChangeInLastYear =>
            IWalletStatsCalculator<FlowTransactionIntervalData>.GetBalanceChangeInLastYear(TurnoverIntervals);

        /// <inheritdoc />
        public decimal WalletTurnover =>
            TurnoverIntervals.Sum(x => Math.Abs((decimal)x.AmountSum)).ToFlow();

        /// <inheritdoc />
        public IEnumerable<TokenBalanceData>? TokenBalances => _tokenBalances?.Any() == true ? _tokenBalances : null;

        /// <inheritdoc />
        public int TokensHolding => _account.TokenBalances?.Edges.Count ?? 0;

        /// <inheritdoc />
        public int DeployedContracts => _account.Contracts.Count;

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
            NativeBalance = balance.ToFlow();
            NativeBalanceUSD = usdBalance;
            _transfers = transfers;
            _account = account;
            _transactions = transactions;
            _nftTransfers = nftTransfers;
            _tokenBalances = tokenBalances;
        }

        /// <inheritdoc />
        public FlowWalletStats Stats()
        {
            return (this as IWalletStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>).ApplyCalculators();
        }

        /// <inheritdoc />
        IWalletTransactionStats IWalletTransactionStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>.Stats()
        {
            if (!_transfers.Any())
            {
                return new FlowWalletStats
                {
                    NoData = true
                };
            }

            var intervals = IWalletStatsCalculator
                .GetTransactionsIntervals(_transfers.Select(x => x.Transaction!.Time)).ToList();
            if (intervals.Count == 0)
            {
                return new FlowWalletStats
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            return new FlowWalletStats
            {
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transfers.Count(t => t.Transaction?.Status?.Equals("Error", StringComparison.InvariantCultureIgnoreCase) == true),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                LastMonthTransactions = _transfers.Count(x => x.Transaction!.Time > monthAgo),
                LastYearTransactions = _transfers.Count(x => x.Transaction!.Time > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transfers.OrderBy(x => x.Transaction?.Time).Last().Transaction!.Time).TotalDays / 30)
            };
        }

        /// <inheritdoc />
        IWalletNftStats IWalletNftStatsCalculator<FlowWalletStats, FlowTransactionIntervalData>.Stats()
        {
            var soldTokens = _nftTransfers
                .Where(x => x.From?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true)
                .ToList();
            var soldSum = IWalletStatsCalculator
                .TokensSum(soldTokens.Select(x => x.Transaction?.Hash!), _transfers.Select(x => (x.Transaction?.Hash!, BigInteger.Parse(x.Amount?.Value!))));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _nftTransfers
                .Where(t => t.To?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(t.GetTokenUid()))
                .ToList();
            var buySum = IWalletStatsCalculator
                .TokensSum(buyTokens.Select(x => x.Transaction?.Hash!), _transfers.Select(x => (x.Transaction?.Hash!, BigInteger.Parse(x.Amount?.Value!))));

            var buyNotSoldTokens = _nftTransfers
                .Where(t => t.To?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(t.GetTokenUid()))
                .ToList();
            var buyNotSoldSum = IWalletStatsCalculator
                .TokensSum(buyNotSoldTokens.Select(x => x.Transaction?.Hash!), _transfers.Select(x => (x.Transaction?.Hash!, BigInteger.Parse(x.Amount?.Value!))));

            int holdingTokens = _nftTransfers.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;

            return new FlowWalletStats
            {
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToFlow(),
                NftWorth = nftWorth.ToFlow()
            };
        }
    }
}