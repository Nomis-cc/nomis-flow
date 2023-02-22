// ------------------------------------------------------------------------------------------------------
// <copyright file="TonStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Tonscan.Interfaces.Extensions;
using Nomis.Tonscan.Interfaces.Models;
using Nomis.Tonscan.Models;
using Nomis.Utils.Extensions;

namespace Nomis.Tonscan.Calculators
{
    /// <summary>
    /// Ton wallet stats calculator.
    /// </summary>
    internal sealed class TonStatCalculator :
        IStatCalculator<TonWalletStats, TonTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<TonscanTransaction> _transactions;
        private readonly IEnumerable<TonscanAccountAssetBalance> _assets;
        private readonly IEnumerable<GetGemsNftItem> _nftAssets;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;

        public TonStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<TonscanTransaction> transactions,
            IEnumerable<TonscanAccountAssetBalance> assets,
            IEnumerable<GetGemsNftItem> nftAssets,
            IEnumerable<TokenBalanceData>? tokenBalances)
        {
            _address = address;
            _balance = balance;
            _usdBalance = usdBalance;
            _transactions = transactions;
            _assets = assets;
            _nftAssets = nftAssets;
            _tokenBalances = tokenBalances;
        }

        public TonWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.Utime.ToString().ToDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var totalTokens = _assets.Select(x => x.Metadata?.Symbol).Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.Utime.ToString().ToDateTime(),
                    x.OutMsgs.Any() ? new BigInteger(x.OutMsgs.Sum(o => (decimal)o.Value)) : x.InMsg?.Value ?? new BigInteger(0),
                    x.OutMsgs.Any()));
            var turnoverIntervals = IStatCalculator<TonTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.Utime.ToString().ToDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance.ToTon(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.Utime.ToString().ToDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = 0,
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => x.OutMsgs.Sum(o => (decimal)o.Value) + (decimal)(x.InMsg?.Value ?? new BigInteger(0))).ToTon(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<TonTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<TonTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.Utime.ToString().ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.Utime.ToString().ToDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.Utime).Last().Utime.ToString().ToDateTime()).TotalDays / 30),
                NftHolding = _nftAssets.Count(),
                /*NftTrading = (soldSum - buySum).ToTon(),
                NftWorth = nftWorth.ToTon(),*/
                TokensHolding = totalTokens.Count(),
                TokenBalances = _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice)
            };
        }
    }
}