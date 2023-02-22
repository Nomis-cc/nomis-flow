// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.WavesExplorer.Extensions;
using Nomis.WavesExplorer.Interfaces.Enums;
using Nomis.WavesExplorer.Interfaces.Extensions;
using Nomis.WavesExplorer.Interfaces.Models;

namespace Nomis.WavesExplorer.Calculators
{
    /// <summary>
    /// Waves wallet stats calculator.
    /// </summary>
    internal sealed class WavesStatCalculator :
        IStatCalculator<WavesWalletStats, WavesTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<WavesExplorerTransaction> _transactions;
        private readonly IEnumerable<WavesExplorerAccountAsset> _assets;
        private readonly IEnumerable<WavesExplorerAccountNftAsset> _nftAssets;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;

        public WavesStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<WavesExplorerTransaction> transactions,
            IEnumerable<WavesExplorerAccountAsset> assets,
            IEnumerable<WavesExplorerAccountNftAsset> nftAssets,
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

        public WavesWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.Timestamp.ToString().ToWavesDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _nftAssets.Where(x => x.Issuer?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokens.Select(x => x.OriginTransactionId!.ToLower()), _transactions.Select(x => (x.Id!.ToLower(), new BigInteger(x.Amount))));

            var soldTokensIds = soldTokens.Select(x => x.AssetId);
            var buyTokens = _nftAssets.Where(x => x.Issuer?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) != true && soldTokensIds.Contains(x.AssetId));
            var buySum = IStatCalculator
                .GetTokensSum(buyTokens.Select(x => x.OriginTransactionId!.ToLower()), _transactions.Select(x => (x.Id!.ToLower(), new BigInteger(x.Amount))));

            var buyNotSoldTokens = _nftAssets.Where(x => x.Issuer?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) != true && !soldTokensIds.Contains(x.AssetId));
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(buyNotSoldTokens.Select(x => x.OriginTransactionId!.ToLower()), _transactions.Select(x => (x.Id!.ToLower(), new BigInteger(x.Amount))));

            int holdingTokens = _nftAssets.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;

            int contractsCreated = _transactions.Count(x => x.Type == (int)WavesExplorerTransactionType.SetScriptTransaction);
            var totalTokens = _assets.Select(x => x.AssetId).Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.Timestamp.ToString().ToWavesDateTime(),
                    new BigInteger(x.Amount),
                    x.Sender?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<WavesTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.Timestamp.ToString().ToWavesDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance.ToWaves(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.Timestamp.ToString().ToWavesDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => t.ApplicationStatus?.Equals("succeeded", StringComparison.InvariantCultureIgnoreCase) == false),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => (decimal)x.Amount).ToWaves(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<WavesTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<WavesTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.Timestamp.ToString().ToWavesDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.Timestamp.ToString().ToWavesDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.Timestamp).Last().Timestamp.ToString().ToWavesDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToWaves(),
                NftWorth = nftWorth.ToWaves(),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens.Count(),
                TokenBalances = _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice)
            };
        }
    }
}