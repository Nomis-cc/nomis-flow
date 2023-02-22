// ------------------------------------------------------------------------------------------------------
// <copyright file="RippleStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.Xrpscan.Interfaces.Extensions;
using Nomis.Xrpscan.Interfaces.Models;

namespace Nomis.Xrpscan.Calculators
{
    /// <summary>
    /// Ripple wallet stats calculator.
    /// </summary>
    internal sealed class RippleStatCalculator :
        IStatCalculator<RippleWalletStats, RippleTransactionIntervalData>
    {
        private readonly XrpscanAccount _account;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly XrpscanKyc _kycStatus;
        private readonly IEnumerable<XrpscanAsset> _assets;
        private readonly IEnumerable<XrpscanOrder> _orders;
        private readonly IEnumerable<XrpscanObligation> _obligations;
        private readonly IEnumerable<XrpscanTransaction> _transactions;
        private readonly IEnumerable<XrpscanNft> _nfts;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;
        private readonly IEnumerable<GreysafeReport>? _greysafeReports;

        public RippleStatCalculator(
            XrpscanAccount account,
            decimal balance,
            decimal usdBalance,
            XrpscanKyc kycStatus,
            IEnumerable<XrpscanAsset> assets,
            IEnumerable<XrpscanOrder> orders,
            IEnumerable<XrpscanObligation> obligations,
            IEnumerable<XrpscanTransaction> transactions,
            IEnumerable<XrpscanNft> nfts,
            IEnumerable<TokenBalanceData>? tokenBalances,
            IEnumerable<GreysafeReport>? greysafeReports)
        {
            _account = account;
            _balance = balance;
            _usdBalance = usdBalance;
            _kycStatus = kycStatus;
            _assets = assets;
            _orders = orders;
            _obligations = obligations;
            _transactions = transactions;
            _nfts = nfts;
            _tokenBalances = tokenBalances;
            _greysafeReports = greysafeReports;
        }

        public RippleWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Where(x => x.Date != null).Select(x => (DateTime)x.Date!)).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _transactions.Where(x => x.TransactionType?.Equals("NFTokenCreateOffer", StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(
                    soldTokens
                        .Select(x => x.Hash!),
                    _transactions
                        .Where(x => x.Amount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true || x.Meta?.DeliveredAmount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true)
                        .Select(x => (x.Hash!, BigInteger.TryParse(x.Amount?.Value?.ToString() ?? x.Meta?.DeliveredAmount?.Value?.ToString(), out var amount) ? amount : 0)));

            var soldTokensIds = soldTokens.Where(x => x.NftTokenId != null).Select(x => x.NftTokenId);
            var buyTokens = _transactions.Where(x => x.TransactionType?.Equals("NFTokenAcceptOffer", StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.NftTokenId));
            var buySum = IStatCalculator
                .GetTokensSum(
                    buyTokens
                        .Select(x => x.Hash!),
                    _transactions
                        .Where(x => x.Amount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true || x.Meta?.DeliveredAmount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true)
                        .Select(x => (x.Hash!, BigInteger.TryParse(x.Amount?.Value?.ToString() ?? x.Meta?.DeliveredAmount?.Value?.ToString(), out var amount) ? amount : 0)));

            var buyNotSoldTokens = _transactions.Where(x => x.TransactionType?.Equals("NFTokenAcceptOffer", StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.NftTokenId));
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(
                    buyNotSoldTokens
                        .Select(x => x.Hash!),
                    _transactions
                        .Where(x => x.Amount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true || x.Meta?.DeliveredAmount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true)
                        .Select(x => (x.Hash!, BigInteger.TryParse(x.Amount?.Value?.ToString() ?? x.Meta?.DeliveredAmount?.Value?.ToString(), out var amount) ? amount : 0)));

            int holdingTokens = _nfts.Count();
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            var totalTokens = _assets.Select(x => x.Currency).Distinct();

            var turnoverIntervalsDataList =
                _transactions
                    .Where(x => x.Amount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true || x.Meta?.DeliveredAmount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true)
                    .Select(x => new TurnoverIntervalsData((DateTime)x.Date!, BigInteger.TryParse(x.Amount?.Value?.ToString() ?? x.Meta?.DeliveredAmount?.Value?.ToString(), out var value) ? value : 0, x.Account?.Equals(_account.Address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<RippleTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => (DateTime)x.Date!)).ToList();

            return new()
            {
                NativeBalance = _balance,
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => (DateTime)x.Date!)),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(x => x.Meta?.TransactionResult?.Equals("tesSUCCESS", StringComparison.InvariantCultureIgnoreCase) != true),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions
                    .Where(x => x.Amount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true || x.Meta?.DeliveredAmount?.Currency?.Equals("XRP", StringComparison.InvariantCultureIgnoreCase) == true)
                    .Sum(x => decimal.TryParse(x.Amount?.Value?.ToString() ?? x.Meta?.DeliveredAmount?.Value?.ToString(), out decimal value) ? value : 0).ToXrp(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<RippleTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<RippleTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.Date > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.Date > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.Where(x => x.Date != null).Min(x => x.Date!.Value)).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToXrp(),
                NftWorth = nftWorth.ToXrp(),
                TokensHolding = totalTokens.Count(),
                TokenBalances = _tokenBalances?.Any() == true ? _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice) : null,
                GreysafeReports = _greysafeReports
            };
        }
    }
}