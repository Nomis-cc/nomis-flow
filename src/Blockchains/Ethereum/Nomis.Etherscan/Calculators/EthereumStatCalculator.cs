// ------------------------------------------------------------------------------------------------------
// <copyright file="EthereumStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using EthScanNet.Lib.Models.ApiResponses.Accounts.Models;
using Nomis.Aave.Interfaces.Responses;
using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Chainanalysis.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Dex.Abstractions.Contracts;
using Nomis.Etherscan.Interfaces.Extensions;
using Nomis.Etherscan.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.HapiExplorer.Interfaces.Responses;
using Nomis.Snapshot.Interfaces.Models;
using Nomis.Utils.Extensions;

namespace Nomis.Etherscan.Calculators
{
    /// <summary>
    /// Ethereum wallet stats calculator.
    /// </summary>
    internal sealed class EthereumStatCalculator :
        IStatCalculator<EthereumWalletStats, EthereumTransactionIntervalData>
    {
        private readonly string _address;
        private readonly BigInteger _balance;
        private readonly decimal _balanceUsd;
        private readonly IEnumerable<EScanTransaction> _transactions;
        private readonly IEnumerable<EScanTransaction> _internalTransactions;
        private readonly IEnumerable<EScanTokenTransferEvent> _tokenTransfers;
        private readonly IEnumerable<EScanTokenTransferEvent> _erc20TokenTransfers;
        private readonly IEnumerable<SnapshotVote>? _snapshotVotes;
        private readonly IEnumerable<SnapshotProposal>? _snapshotProposals;
        private readonly HapiProxyRiskScoreResponse? _hapiRiskScore;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;
        private readonly IEnumerable<DexTokenSwapPairsData> _dexTokenSwapPairs;
        private readonly AaveUserAccountDataResponse? _aaveUserAccountData;
        private readonly IEnumerable<GreysafeReport>? _greysafeReports;
        private readonly IEnumerable<ChainanalysisReport>? _chainanalysisReports;

        public EthereumStatCalculator(
            string address,
            BigInteger balance,
            decimal balanceUsd,
            IEnumerable<EScanTransaction> transactions,
            IEnumerable<EScanTransaction> internalTransactions,
            IEnumerable<EScanTokenTransferEvent> tokenTransfers,
            IEnumerable<EScanTokenTransferEvent> erc20TokenTransfers,
            IEnumerable<SnapshotVote>? snapshotVotes,
            IEnumerable<SnapshotProposal>? snapshotProposals,
            HapiProxyRiskScoreResponse? hapiRiskScore,
            IEnumerable<TokenBalanceData>? tokenBalances,
            IEnumerable<DexTokenSwapPairsData> dexTokenSwapPairs,
            AaveUserAccountDataResponse? aaveUserAccountData,
            IEnumerable<GreysafeReport>? greysafeReports,
            IEnumerable<ChainanalysisReport>? chainanalysisReports)
        {
            _address = address;
            _balance = balance;
            _balanceUsd = balanceUsd;
            _transactions = transactions;
            _internalTransactions = internalTransactions;
            _tokenTransfers = tokenTransfers;
            _erc20TokenTransfers = erc20TokenTransfers;
            _snapshotVotes = snapshotVotes;
            _snapshotProposals = snapshotProposals;
            _hapiRiskScore = hapiRiskScore;
            _tokenBalances = tokenBalances;
            _dexTokenSwapPairs = dexTokenSwapPairs;
            _aaveUserAccountData = aaveUserAccountData;
            _greysafeReports = greysafeReports;
            _chainanalysisReports = chainanalysisReports;
        }

        public EthereumWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.TimeStamp!.ToDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _tokenTransfers.Where(x => x.From?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokens.Select(x => x.Hash), _internalTransactions.Select(x => (x.Hash, x.Value)));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _tokenTransfers.Where(x => x.To?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            var buySum = IStatCalculator
                .GetTokensSum(buyTokens.Select(x => x.Hash), _internalTransactions.Select(x => (x.Hash, x.Value)));

            var buyNotSoldTokens = _tokenTransfers.Where(x => x.To?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.GetTokenUid()));
            var buyNotSoldSum = IStatCalculator
                .GetTokensSum(buyNotSoldTokens.Select(x => x.Hash), _internalTransactions.Select(x => (x.Hash, x.Value)));

            int holdingTokens = _tokenTransfers.Count() - soldTokens.Count;
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            int contractsCreated = _transactions.Count(x => !string.IsNullOrWhiteSpace(x.ContractAddress));
            var totalTokens = _erc20TokenTransfers.Select(x => x.TokenSymbol).Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.TimeStamp.ToDateTime(),
                    x.Value,
                    x.From?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<EthereumTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.TimeStamp.ToDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance.ToEth(),
                NativeBalanceUSD = _balanceUsd,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.TimeStamp!.ToDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => string.Equals(t.TxreceiptStatus, "0", StringComparison.OrdinalIgnoreCase) || string.Equals(t.IsError, "1", StringComparison.OrdinalIgnoreCase)),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => (decimal)x.Value).ToEth(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<EthereumTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<EthereumTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.TimeStamp.ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.TimeStamp.ToDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.TimeStamp).Last().TimeStamp.ToDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToEth(),
                NftWorth = nftWorth.ToEth(),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens.Count(),
                SnapshotVotes = IStatCalculator.GetSnapshotProtocolVotesData(_snapshotVotes),
                SnapshotProposals = IStatCalculator.GetSnapshotProtocolProposalsData(_snapshotProposals),
                HapiRiskScore = _hapiRiskScore,
                TokenBalances = _tokenBalances?.Any() == true ? _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice) : null,
                DexTokensSwapPairs = _dexTokenSwapPairs.Any() ? _dexTokenSwapPairs : null,
                AaveData = _aaveUserAccountData,
                GreysafeReports = _greysafeReports?.Any() == true ? _greysafeReports : null,
                ChainanalysisReports = _chainanalysisReports?.Any() == true ? _chainanalysisReports : null
            };
        }
    }
}