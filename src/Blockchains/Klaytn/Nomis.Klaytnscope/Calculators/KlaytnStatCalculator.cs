// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Chainanalysis.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.Klaytnscope.Interfaces.Extensions;
using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Snapshot.Interfaces.Models;
using Nomis.Utils.Extensions;

namespace Nomis.Klaytnscope.Calculators
{
    /// <summary>
    /// Klaytn wallet stats calculator.
    /// </summary>
    internal sealed class KlaytnStatCalculator :
        IStatCalculator<KlaytnWalletStats, KlaytnTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<KlaytnscopeAccountNormalTransaction> _transactions;
        private readonly IEnumerable<KlaytnscopeAccountInternalTransaction> _internalTransactions;
        private readonly IEnumerable<KlaytnscopeAccountNftTransfer> _nftTransfers;
        private readonly IEnumerable<KlaytnscopeAccountKIP17TokenEvent> _kip17TokenTransfers;
        private readonly IEnumerable<SnapshotVote>? _snapshotVotes;
        private readonly IEnumerable<SnapshotProposal>? _snapshotProposals;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;
        private readonly IEnumerable<GreysafeReport>? _greysafeReports;
        private readonly IEnumerable<ChainanalysisReport>? _chainanalysisReports;

        public KlaytnStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<KlaytnscopeAccountNormalTransaction> transactions,
            IEnumerable<KlaytnscopeAccountInternalTransaction> internalTransactions,
            IEnumerable<KlaytnscopeAccountNftTransfer> nftTransfers,
            IEnumerable<KlaytnscopeAccountKIP17TokenEvent> kip17TokenTransfers,
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
            _internalTransactions = internalTransactions;
            _nftTransfers = nftTransfers;
            _kip17TokenTransfers = kip17TokenTransfers;
            _snapshotVotes = snapshotVotes;
            _snapshotProposals = snapshotProposals;
            _tokenBalances = tokenBalances;
            _greysafeReports = greysafeReports;
            _chainanalysisReports = chainanalysisReports;
        }

        public KlaytnWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.CreatedAt.ToString().ToDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _nftTransfers.Where(x => x.FromAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokens.Select(x => x.BlockNumber.ToString()), _internalTransactions.Select(x => (x.BlockNumber.ToString(), BigInteger.TryParse(x.Amount, out var amount) ? amount : 0)));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _nftTransfers.Where(x => x.ToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            var buySum = IStatCalculator
                .GetTokensSum(buyTokens.Select(x => x.BlockNumber.ToString()), _internalTransactions.Select(x => (x.BlockNumber.ToString(), BigInteger.TryParse(x.Amount, out var amount) ? amount : 0)));

            var buyNotSoldTokens = _nftTransfers.Where(x => x.ToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) != true && !soldTokensIds.Contains(x.GetTokenUid()));
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(buyNotSoldTokens.Select(x => x.BlockNumber.ToString()), _internalTransactions.Select(x => (x.BlockNumber.ToString(), BigInteger.TryParse(x.Amount, out var amount) ? amount : 0)));

            int holdingTokens = _nftTransfers.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            int contractsCreated = _transactions.Count(x => x.TxType?.Equals("contract_create", StringComparison.CurrentCultureIgnoreCase) == true);
            var totalTokens = _kip17TokenTransfers.Select(x => x.TokenAddress).Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.CreatedAt.ToString().ToDateTime(),
                    BigInteger.TryParse(x.Amount, out var amount) ? amount : 0,
                    x.FromAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<KlaytnTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.CreatedAt.ToString().ToDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance.ToKlay(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.CreatedAt.ToString().ToDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => t.TxStatus == 0),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => decimal.TryParse(x.Amount, out decimal amount) ? amount.ToKlay() : 0),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<KlaytnTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<KlaytnTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.CreatedAt.ToString().ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.CreatedAt.ToString().ToDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.CreatedAt).Last().CreatedAt.ToString().ToDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToKlay(),
                NftWorth = nftWorth.ToKlay(),
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