// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Nomis.Blockchain.Abstractions.Stats;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Stats;
using Nomis.Utils.Contracts;
using Nomis.Utils.Contracts.Stats;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flow wallet stats.
    /// </summary>
    public sealed class FlowWalletStats :
        IWalletCommonStats<FlowTransactionIntervalData>,
        IWalletNativeBalanceStats,
        IWalletTokenBalancesStats,
        IWalletTransactionStats,
        IWalletNftStats,
        IWalletTokenStats,
        IWalletContractStats
    {
        /// <inheritdoc/>
        public bool NoData { get; set; }

        /// <inheritdoc/>
        public string NativeToken => "Flow";

        /// <inheritdoc/>
        [Display(Description = "Amount of deployed smart-contracts", GroupName = "number")]
        public int DeployedContracts { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Wallet native token balance", GroupName = "Native token")]
        public decimal NativeBalance { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Wallet native token balance", GroupName = "USD")]
        public decimal NativeBalanceUSD { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Wallet hold tokens total balance", GroupName = "USD")]
        public decimal HoldTokensBalanceUSD => TokenBalances?.Sum(b => b.TotalAmountPrice) ?? 0;

        /// <inheritdoc/>
        [Display(Description = "Wallet age", GroupName = "months")]
        public int WalletAge { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Total transactions on wallet", GroupName = "number")]
        public int TotalTransactions { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Total rejected transactions on wallet", GroupName = "number")]
        public int TotalRejectedTransactions { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Average time interval between transactions", GroupName = "hours")]
        public double AverageTransactionTime { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Maximum time interval between transactions", GroupName = "hours")]
        public double MaxTransactionTime { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Minimal time interval between transactions", GroupName = "hours")]
        public double MinTransactionTime { get; set; }

        /// <inheritdoc/>
        [Display(Description = "The movement of funds on the wallet", GroupName = "Native token")]
        public decimal WalletTurnover { get; set; }

        /// <inheritdoc/>
        public IEnumerable<FlowTransactionIntervalData>? TurnoverIntervals { get; set; }

        /// <inheritdoc/>
        [Display(Description = "The balance change value in the last month", GroupName = "Native token")]
        public decimal BalanceChangeInLastMonth { get; set; }

        /// <inheritdoc/>
        [Display(Description = "The balance change value in the last year", GroupName = "Native token")]
        public decimal BalanceChangeInLastYear { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Total NFTs on wallet", GroupName = "number")]
        public int NftHolding { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Time since last transaction", GroupName = "months")]
        public int TimeFromLastTransaction { get; set; }

        /// <inheritdoc/>
        [Display(Description = "NFT trading activity", GroupName = "Native token")]
        public decimal NftTrading { get; set; }

        /// <inheritdoc/>
        [Display(Description = "NFT worth on wallet", GroupName = "Native token")]
        public decimal NftWorth { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Last month transactions", GroupName = "number")]
        public int LastMonthTransactions { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Last year transactions on wallet", GroupName = "number")]
        public int LastYearTransactions { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Average transaction per months", GroupName = "number")]
        public double TransactionsPerMonth => WalletAge != 0 ? (double)TotalTransactions / WalletAge : 0;

        /// <inheritdoc/>
        [Display(Description = "Value of all holding tokens", GroupName = "number")]
        public int TokensHolding { get; set; }

        /// <inheritdoc/>
        [Display(Description = "Hold tokens balances", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<TokenBalanceData>? TokenBalances { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, PropertyData> StatsDescriptions => GetType()
            .GetProperties()
            .Where(prop => !ExcludedStatDescriptions.Contains(prop.Name) && Attribute.IsDefined(prop, typeof(DisplayAttribute)) && !Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)))
            .ToDictionary(p => p.Name, p => new PropertyData(p, NativeToken));

        /// <inheritdoc cref="IWalletCommonStats{ITransactionIntervalData}.ExcludedStatDescriptions"/>
        [JsonIgnore]
        public IEnumerable<string> ExcludedStatDescriptions =>
            IWalletCommonStats<FlowTransactionIntervalData>.ExcludedStatDescriptions.Union(new List<string>
            {
                nameof(TokenBalances)
            });
    }
}