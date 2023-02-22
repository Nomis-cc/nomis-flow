// ------------------------------------------------------------------------------------------------------
// <copyright file="CscStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Chainanalysis.Interfaces.Models;
using Nomis.CscExplorer.Interfaces.Extensions;
using Nomis.CscExplorer.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.Snapshot.Interfaces.Models;
using Nomis.Utils.Extensions;

namespace Nomis.CscExplorer.Calculators
{
    /// <summary>
    /// Csc wallet stats calculator.
    /// </summary>
    internal sealed class CscStatCalculator :
        IStatCalculator<CscWalletStats, CscTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<CscExplorerAccountTransactionRecord> _transactions;
        private readonly IEnumerable<CscExplorerAccountCetTransferRecord> _cetTransfers;
        private readonly IEnumerable<CscExplorerAccountCrc20TransferRecord> _crc20Transfers;
        private readonly IEnumerable<CscExplorerAccountCrc721TransferRecord> _crc721Transfers;
        private readonly IEnumerable<SnapshotVote>? _snapshotVotes;
        private readonly IEnumerable<SnapshotProposal>? _snapshotProposals;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;
        private readonly IEnumerable<GreysafeReport>? _greysafeReports;
        private readonly IEnumerable<ChainanalysisReport>? _chainanalysisReports;

        public CscStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<CscExplorerAccountTransactionRecord> transactions,
            IEnumerable<CscExplorerAccountCetTransferRecord> cetTransfers,
            IEnumerable<CscExplorerAccountCrc20TransferRecord> crc20Transfers,
            IEnumerable<CscExplorerAccountCrc721TransferRecord> crc721Transfers,
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
            _cetTransfers = cetTransfers;
            _crc20Transfers = crc20Transfers;
            _crc721Transfers = crc721Transfers;
            _snapshotVotes = snapshotVotes;
            _snapshotProposals = snapshotProposals;
            _tokenBalances = tokenBalances;
            _greysafeReports = greysafeReports;
            _chainanalysisReports = chainanalysisReports;
        }

        public CscWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.TimeStamp.ToString().ToDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _crc721Transfers.Where(x => x.From?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokens.Select(x => x.TxHash!), _transactions.Select(x => (x.TxHash!, BigInteger.TryParse(x.Amount, out var amount) ? amount : 0)));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _crc721Transfers.Where(x => x.To?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            var buySum = IStatCalculator
                .GetTokensSum(buyTokens.Select(x => x.TxHash!), _transactions.Select(x => (x.TxHash!, BigInteger.TryParse(x.Amount, out var amount) ? amount : 0)));

            var buyNotSoldTokens = _crc721Transfers.Where(x => x.To?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.GetTokenUid()));
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(buyNotSoldTokens.Select(x => x.TxHash!), _transactions.Select(x => (x.TxHash!, BigInteger.TryParse(x.Amount, out var amount) ? amount : 0)));

            int holdingTokens = _crc721Transfers.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            int contractsCreated = _transactions.Count(x => x.Method?.Equals("Create Contract") == true && x.Status == 1);
            var totalTokens = _crc20Transfers.Select(x => x.TokenInfo?.Symbol).Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.TimeStamp.ToString().ToDateTime(),
                    BigInteger.TryParse(x.Amount, out var value) ? value : 0,
                    x.From?.Address?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<CscTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.TimeStamp.ToString().ToDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance,
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.TimeStamp.ToString().ToDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => t.Status == 0),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => decimal.TryParse(x.Amount, out decimal value) ? value : 0),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<CscTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<CscTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.TimeStamp.ToString().ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.TimeStamp.ToString().ToDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.TimeStamp).Last().TimeStamp.ToString().ToDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (decimal)(soldSum - buySum),
                NftWorth = nftWorth,
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