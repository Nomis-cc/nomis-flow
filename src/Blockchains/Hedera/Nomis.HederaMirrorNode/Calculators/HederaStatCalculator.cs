// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.HederaMirrorNode.Extensions;
using Nomis.HederaMirrorNode.Interfaces.Extensions;
using Nomis.HederaMirrorNode.Interfaces.Models;

namespace Nomis.HederaMirrorNode.Calculators
{
    /// <summary>
    /// Hedera wallet stats calculator.
    /// </summary>
    internal sealed class HederaStatCalculator :
        IStatCalculator<HederaWalletStats, HederaTransactionIntervalData>
    {
        private readonly string _address;
        private readonly HederaMirrorNodeAccount _accountData;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<HederaMirrorNodeAccountTransaction> _transactions;
        private readonly IEnumerable<HederaMirrorNodeAccountTransactionTransfer> _internalTransactions;
        private readonly IEnumerable<HederaMirrorNodeNftData> _nfts;
        private readonly IEnumerable<HederaMirrorNodeNftTransactionData> _nftTransactions;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;

        public HederaStatCalculator(
            string address,
            HederaMirrorNodeAccount accountData,
            decimal usdBalance,
            IEnumerable<HederaMirrorNodeAccountTransaction> transactions,
            IEnumerable<HederaMirrorNodeAccountTransactionTransfer> internalTransactions,
            IEnumerable<HederaMirrorNodeNftData> nfts,
            IEnumerable<HederaMirrorNodeNftTransactionData> nftTransactions,
            IEnumerable<TokenBalanceData>? tokenBalances)
        {
            _address = address;
            _accountData = accountData;
            _usdBalance = usdBalance;
            _transactions = transactions;
            _internalTransactions = internalTransactions;
            _nfts = nfts;
            _nftTransactions = nftTransactions;
            _tokenBalances = tokenBalances;
        }

        public HederaWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.ConsensusTimestamp!.ToHederaDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _nftTransactions
                .Where(x => x.Type?.Equals("CRYPTOTRANSFER") == true
                            && x.SenderAccountId?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true)
                .ToList();
            var soldTokensHashes = soldTokens.Select(x => x.TransactionId!);
            var soldTokensTransactions = _transactions
                .Where(x => x.Name?.Equals("CRYPTOTRANSFER") == true
                            && soldTokensHashes.Contains(x.TransactionId)
                            && x.Transfers.Any(t => t.Account?.Equals(_address) == true));
            var soldTokensTransactionSums = soldTokensTransactions
                .Select(x => (x.TransactionId!, x.Transfers
                    .Where(t => t.Account?.Equals(_address) == true)
                    .Sum(t => t.Amount)));
            var soldSum = IStatCalculator
                .GetTokensSum(soldTokensHashes, soldTokensTransactionSums);

            var buyTokens = _nftTransactions
                .Where(x => x.Type?.Equals("CRYPTOTRANSFER") == true
                            && x.ReceiverAccountId?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true)
                .ToList();
            var buyTokensHashes = buyTokens.Select(x => x.TransactionId!);
            var buyTokensTransactions = _transactions
                .Where(x => x.Name?.Equals("CRYPTOTRANSFER") == true
                            && buyTokensHashes.Contains(x.TransactionId)
                            && x.Transfers.Any(t => t.Account?.Equals(_address) == true));
            var buyTokensTransactionSums = buyTokensTransactions
                .Select(x => (x.TransactionId!, x.Transfers
                    .Where(t => t.Account?.Equals(_address) == true)
                    .Sum(t => -t.Amount)));
            var buySum = IStatCalculator
                .GetTokensSum(buyTokensHashes, buyTokensTransactionSums);

            /*var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _nftTransactions.Where(x => x.To?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            var buySum = IStatCalculator<HederaWalletStats, HederaTransactionIntervalData>
                .GetTokensSum(buyTokens.Select(x => x.Hash!), _internalTransactions.Select(x => (x.Hash!, BigInteger.TryParse(x.Value, out var amount) ? amount : 0)));*/

            /*var buyNotSoldTokens = _nftTransactions.Where(x => x.To?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.GetTokenUid()));
            var buyNotSoldSum = IStatCalculator<HederaWalletStats, HederaTransactionIntervalData>
                .GetTokensSum(buyNotSoldTokens.Select(x => x.Hash!), _internalTransactions.Select(x => (x.Hash!, BigInteger.TryParse(x.Value, out var amount) ? amount : 0)));*/

            int holdingTokens = _nfts.Count() - soldTokens.Count;
            /*var nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;*/

            int contractsCreated = _transactions.Count(x => x.Name?.Equals("CONTRACTCREATEINSTANCE") == true);
            int totalTokens = _accountData?.Balance?.Tokens.Count(t => t.Balance > 0) ?? 0;

            var turnoverIntervalsDataList =
                _transactions.Where(t => t.Name?.Equals("CRYPTOTRANSFER") == true && t.Result?.Equals("SUCCESS") == true)
                    .Select(x => new TurnoverIntervalsData(
                        x.ConsensusTimestamp!.ToHederaDateTime(),
                        BigInteger.TryParse(
                            x.Transfers
                                    .Where(t => t.Account?.Equals(_address) == true)
                                    .Sum(t => Math.Abs((decimal)t.Amount)).ToString(CultureInfo.InvariantCulture), out var value) ? value : 0,
                        x.Transfers.Where(t => t.Account?.Equals(_address) == true).Sum(t => t.Amount) > 0));
            var turnoverIntervals = IStatCalculator<HederaTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.ConsensusTimestamp!.ToHederaDateTime())).ToList();

            return new()
            {
                NativeBalance = ((decimal)(_accountData?.Balance?.Balance ?? 0)).ToHbars(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.ConsensusTimestamp!.ToHederaDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => !string.Equals(t.Result, "SUCCESS", StringComparison.OrdinalIgnoreCase)),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = (BigInteger.TryParse(
                    _transactions.Sum(x => x.Transfers.Where(t => t.Account?.Equals(_address) != true)
                    .Sum(t => Math.Abs((decimal)t.Amount))).ToString(CultureInfo.InvariantCulture), out var value) ? value : 0).ToHbars(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<HederaTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<HederaTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.ConsensusTimestamp!.ToHederaDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.ConsensusTimestamp!.ToHederaDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.ConsensusTimestamp).Last().ConsensusTimestamp!.ToHederaDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = Math.Abs((decimal)soldSum - (decimal)buySum).ToHbars(),
                /*NftWorth = nftWorth.ToH(),*/
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens,
                TokenBalances = _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice)
            };
        }
    }
}