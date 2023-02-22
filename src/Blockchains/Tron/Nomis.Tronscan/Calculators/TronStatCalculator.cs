// ------------------------------------------------------------------------------------------------------
// <copyright file="TronStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.HapiExplorer.Interfaces.Responses;
using Nomis.Tronscan.Extensions;
using Nomis.Tronscan.Interfaces.Extensions;
using Nomis.Tronscan.Interfaces.Models;

namespace Nomis.Tronscan.Calculators
{
    /// <summary>
    /// Tron wallet stats calculator.
    /// </summary>
    internal sealed class TronStatCalculator :
        IStatCalculator<TronWalletStats, TronTransactionIntervalData>
    {
        private readonly string _address;
        private readonly TronscanAccount _accountData;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<TronscanAccountNormalTransaction> _transactions;
        private readonly IEnumerable<TronscanAccountInternalTransaction> _internalTransactions;
        private readonly IEnumerable<TronscanAccountTransfer> _tokenTransfers;
        private readonly IEnumerable<TronscanAccountTrc20TokenBalance> _trc20TokenTransfers;
        private readonly TronscanAccountContracts _contractsData;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;
        private readonly HapiProxyRiskScoreResponse? _hapiRiskScore;
        private readonly IEnumerable<GreysafeReport>? _greysafeReports;

        public TronStatCalculator(
            string address,
            TronscanAccount accountData,
            decimal balance,
            decimal usdBalance,
            IEnumerable<TronscanAccountNormalTransaction> transactions,
            IEnumerable<TronscanAccountInternalTransaction> internalTransactions,
            IEnumerable<TronscanAccountTransfer> tokenTransfers,
            IEnumerable<TronscanAccountTrc20TokenBalance> trc20TokenTransfers,
            TronscanAccountContracts contractsData,
            IEnumerable<TokenBalanceData>? tokenBalances,
            HapiProxyRiskScoreResponse? hapiRiskScore,
            IEnumerable<GreysafeReport>? greysafeReports)
        {
            _address = address;
            _accountData = accountData;
            _balance = balance;
            _usdBalance = usdBalance;
            _transactions = transactions;
            _internalTransactions = internalTransactions;
            _tokenTransfers = tokenTransfers;
            _trc20TokenTransfers = trc20TokenTransfers;
            _contractsData = contractsData;
            _tokenBalances = tokenBalances;
            _hapiRiskScore = hapiRiskScore;
            _greysafeReports = greysafeReports;
        }

        public TronWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.Timestamp.ToTronDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _tokenTransfers.Where(x => x.TransferFromAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokens.Select(x => x.TransactionHash!), _internalTransactions.Select(x => (x.Hash!, new BigInteger(x.CallValue))));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _tokenTransfers.Where(x => x.TransferToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            var buySum = IStatCalculator
                .GetTokensSum(buyTokens.Select(x => x.TransactionHash!), _internalTransactions.Select(x => (x.Hash!, new BigInteger(x.CallValue))));

            var buyNotSoldTokens = _tokenTransfers.Where(x => x.TransferToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.GetTokenUid()));
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(buyNotSoldTokens.Select(x => x.TransactionHash!), _internalTransactions.Select(x => (x.Hash!, new BigInteger(x.CallValue))));

            int holdingTokens = _tokenTransfers.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            int contractsCreated = _contractsData.Total;

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.Timestamp.ToTronDateTime(),
                    new BigInteger(x.Amount),
                    x.OwnerAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<TronTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.Timestamp.ToTronDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance,
                NativeBalanceUSD = _usdBalance,
                WalletAge = (int)((DateTime.UtcNow - _accountData.DateCreated.ToTronDateTime()).TotalDays / 30),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => !t.Confirmed),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => x.Amount).ToTrx(),
                BalanceChangeInLastMonth = IStatCalculator<TronTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<TronTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                TurnoverIntervals = turnoverIntervals,
                LastMonthTransactions = _transactions.Count(x => x.Timestamp.ToTronDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.Timestamp.ToTronDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.Timestamp.ToString()).Last().Timestamp.ToTronDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToTrx(),
                NftWorth = nftWorth.ToTrx(),
                DeployedContracts = contractsCreated,
                TokensHolding = _accountData.Tokens?.Count ?? 0,
                TokenBalances = _tokenBalances?.Any() == true ? _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice) : null,
                HapiRiskScore = _hapiRiskScore,
                GreysafeReports = _greysafeReports
            };
        }
    }
}