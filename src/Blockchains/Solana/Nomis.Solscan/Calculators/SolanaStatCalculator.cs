// ------------------------------------------------------------------------------------------------------
// <copyright file="SolanaStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.HapiExplorer.Interfaces.Responses;
using Nomis.MagicEden.Interfaces.Models;
using Nomis.Solscan.Interfaces.Extensions;
using Nomis.Solscan.Interfaces.Models;
using Nomis.Utils.Exceptions;
using Nomis.Utils.Extensions;

namespace Nomis.Solscan.Calculators
{
    /// <summary>
    /// Solana wallet stats calculator.
    /// </summary>
    internal sealed class SolanaStatCalculator :
        IStatCalculator<SolanaWalletStats, SolanaTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<MagicEdenWalletActivity> _magicEdenWalletActivities;
        private readonly SolscanSplTransferList _splTransfers;
        private readonly SolscanSolTransferList _solTransfers;
        private readonly List<SolscanToken> _tokens;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;
        private readonly HapiProxyRiskScoreResponse? _hapiRiskScore;
        private readonly IEnumerable<GreysafeReport>? _greysafeReports;

        public SolanaStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<MagicEdenWalletActivity> magicEdenWalletActivities,
            SolscanSplTransferList splTransfers,
            SolscanSolTransferList solTransfers,
            List<SolscanToken> tokens,
            IEnumerable<TokenBalanceData>? tokenBalances,
            HapiProxyRiskScoreResponse? hapiRiskScore,
            IEnumerable<GreysafeReport>? greysafeReports)
        {
            _address = address;
            _balance = balance;
            _usdBalance = usdBalance;
            _magicEdenWalletActivities = magicEdenWalletActivities;
            _splTransfers = splTransfers;
            _solTransfers = solTransfers;
            _tokens = tokens;
            _tokenBalances = tokenBalances;
            _hapiRiskScore = hapiRiskScore;
            _greysafeReports = greysafeReports;
        }

        public SolanaWalletStats GetStats()
        {
            var splTransferIntervals = IStatCalculator
                .GetTransactionsIntervals(_splTransfers.Data.Select(x => x.Date)).ToList();
            var solTransferIntervals = IStatCalculator
                .GetTransactionsIntervals(_solTransfers.Data.Select(x => x.Date)).ToList();
            var allTransferIntervals = splTransferIntervals.Union(solTransferIntervals).ToList();

            if (allTransferIntervals.Count == 0)
            {
                throw new NoDataException("There is no transfers for this wallet");
            }

            var splTransferBlockTimes = _splTransfers.Data.Select(x => x.BlockTime.ToString().ToDateTime());
            var solTransferBlockTimes = _solTransfers.Data.Select(x => x.BlockTime.ToString().ToDateTime());
            var allTransferBlockTimes = splTransferBlockTimes.Union(solTransferBlockTimes).ToList();

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);
            var magicEdenWalletBuys = _magicEdenWalletActivities
                .Where(x => x.Type?.Equals("buyNow") == true && x.Buyer?.Equals(_address) == true).ToList();
            var magicEdenWalletSells = _magicEdenWalletActivities
                .Where(x => x.Type?.Equals("buyNow") == true && x.Seller?.Equals(_address) == true).ToList();

            var turnoverIntervalsDataList =
                _solTransfers.Data.Select(x => new TurnoverIntervalsData(
                    x.BlockTime.ToString().ToDateTime(),
                    new(x.Lamport),
                    x.Src?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<SolanaTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _solTransfers.Data.Min(x => x.BlockTime.ToString().ToDateTime())).ToList();

            var nftWorthItems = magicEdenWalletBuys
                .Where(x => !magicEdenWalletSells.Select(y => y.TokenMint).Contains(x.TokenMint))
                .ToList();
            decimal nftWorth = nftWorthItems.Count > 0
                ? nftWorthItems.Sum(x => x.Price)
                : 0;

            return new()
            {
                NativeBalance = _balance,
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(allTransferBlockTimes),
                TotalTransactions = _splTransfers.Total + _solTransfers.Data.Count,
                MinTransactionTime = allTransferIntervals.Min(),
                MaxTransactionTime = allTransferIntervals.Max(),
                AverageTransactionTime = allTransferIntervals.Average(),
                WalletTurnover = _solTransfers.Data.Sum(x => x.Lamport).ToSol(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<SolanaTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<SolanaTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = (_splTransfers.Data.Count > 0 ? _splTransfers.Data.Count(x => x.Date > monthAgo) : 0)
                                        + _solTransfers.Data.Count(x => x.Date > monthAgo),
                LastYearTransactions = (_splTransfers.Data.Count > 0 ? _splTransfers.Data.Count(x => x.Date > yearAgo) : 0)
                                       + _solTransfers.Data.Count(x => x.Date > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - GetMaxBlockTime(
                    _splTransfers.Data.Count > 0 ? _splTransfers.Data.Min(x => x.BlockTime) : long.MinValue,
                    _solTransfers.Data.Count > 0 ? _solTransfers.Data.Min(x => x.BlockTime) : long.MinValue).ToString().ToDateTime()).TotalDays / 30),
                NftHolding = _tokens.Count(x => x.TokenAmount is { Decimals: 0, UiAmount: > 0 }),
                NftTrading = (magicEdenWalletSells.Count > 0 ? magicEdenWalletSells.Sum(x => x.Price) : 0) - (magicEdenWalletBuys.Count > 0 ? magicEdenWalletBuys.Sum(x => x.Price) : 0),
                NftWorth = nftWorth,
                TokensHolding = _tokens.Count(x => x.TokenAmount?.UiAmount > 0),
                TokenBalances = _tokenBalances,
                HapiRiskScore = _hapiRiskScore,
                GreysafeReports = _greysafeReports
            };
        }

        private static long GetMaxBlockTime(params long[] blockTimes)
        {
            long result = long.MinValue;
            foreach (long blockTime in blockTimes)
            {
                result = Math.Max(result, blockTime);
            }

            return result;
        }
    }
}