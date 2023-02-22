// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.AeternityExplorer.Extensions;
using Nomis.AeternityExplorer.Interfaces.Extensions;
using Nomis.AeternityExplorer.Interfaces.Models;
using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;

namespace Nomis.AeternityExplorer.Calculators
{
    /// <summary>
    /// Aeternity wallet stats calculator.
    /// </summary>
    internal sealed class AeternityStatCalculator :
        IStatCalculator<AeternityWalletStats, AeternityTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<AeternityExplorerAccountNormalTransaction> _transactions;
        private readonly IEnumerable<AeternityExplorerAccountInternalTransaction> _internalTransactions;
        private readonly IEnumerable<AeternityExplorerAccountAEX141TokenEvent> _aex141TokenTransfers;
        private readonly IEnumerable<AeternityExplorerAccountAEX9TokenEvent> _aex9TokenTransfers;

        public AeternityStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<AeternityExplorerAccountNormalTransaction> transactions,
            IEnumerable<AeternityExplorerAccountInternalTransaction> internalTransactions,
            IEnumerable<AeternityExplorerAccountAEX141TokenEvent> aex141TokenTransfers,
            IEnumerable<AeternityExplorerAccountAEX9TokenEvent> aex9TokenTransfers)
        {
            _address = address;
            _balance = balance;
            _usdBalance = usdBalance;
            _transactions = transactions;
            _internalTransactions = internalTransactions;
            _aex141TokenTransfers = aex141TokenTransfers;
            _aex9TokenTransfers = aex9TokenTransfers;
        }

        public AeternityWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.MicroTime.ToString().ToAeternityDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _aex141TokenTransfers.Where(x => x.Sender?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokens.Select(x => x.BlockHeight.ToString()), _internalTransactions.Select(x => (x.Height.ToString(), x.Amount)));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _aex141TokenTransfers.Where(x => x.Recipient?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            var buySum = IStatCalculator
                .GetTokensSum(buyTokens.Select(x => x.BlockHeight.ToString()), _internalTransactions.Select(x => (x.Height.ToString(), x.Amount)));

            var buyNotSoldTokens = _aex141TokenTransfers.Where(x => x.Recipient?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.GetTokenUid()));
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(buyNotSoldTokens.Select(x => x.BlockHeight.ToString()), _internalTransactions.Select(x => (x.Height.ToString(), x.Amount)));

            int holdingTokens = _aex141TokenTransfers.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            int contractsCreated = _transactions.Count(x => x.Transaction?.Type?.Equals("contract_create", StringComparison.CurrentCultureIgnoreCase) == true);
            var totalTokens = _aex9TokenTransfers.Select(x => x.ContractId).Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.MicroTime.ToString().ToAeternityDateTime(),
                    x.Transaction?.Amount ?? new BigInteger(0),
                    x.Transaction?.From?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<AeternityTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.MicroTime.ToString().ToAeternityDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance.ToAeternity(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.MicroTime.ToString().ToAeternityDateTime())),
                TotalTransactions = _transactions.Count(),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => (decimal?)x.Transaction?.Amount ?? 0).ToAeternity(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<AeternityTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<AeternityTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.MicroTime.ToString().ToAeternityDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.MicroTime.ToString().ToAeternityDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.MicroTime).Last().MicroTime.ToString().ToAeternityDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToAeternity(),
                NftWorth = nftWorth.ToAeternity(),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens.Count()
            };
        }
    }
}