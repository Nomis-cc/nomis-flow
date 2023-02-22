// ------------------------------------------------------------------------------------------------------
// <copyright file="BscWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Dex.Abstractions.Contracts;
using Nomis.Dex.Abstractions.Stats;
using Nomis.HapiExplorer.Interfaces.Responses;
using Nomis.HapiExplorer.Interfaces.Stats;

namespace Nomis.Bscscan.Interfaces.Models
{
    /// <summary>
    /// Bsc wallet stats.
    /// </summary>
    public sealed class BscWalletStats :
        BaseEvmWalletStats<BscTransactionIntervalData>,
        IWalletDexTokenSwapPairsStats,
        IWalletNftStats,
        IWalletHapiStats
    {
        /// <inheritdoc/>
        public override string NativeToken => "BNB";

        /// <inheritdoc/>
        [Display(Description = "Total NFTs on wallet", GroupName = "number")]
        public int NftHolding { get; init; }

        /// <inheritdoc/>
        [Display(Description = "NFT trading activity", GroupName = "Native token")]
        public decimal NftTrading { get; init; }

        /// <inheritdoc/>
        [Display(Description = "NFT worth on wallet", GroupName = "Native token")]
        public decimal NftWorth { get; init; }

        /// <inheritdoc/>
        [Display(Description = "The HAPI protocol risk score data", GroupName = "value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HapiProxyRiskScoreResponse? HapiRiskScore { get; init; }

        /// <inheritdoc/>
        [Display(Description = "DEX tokens balances", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<DexTokenSwapPairsData>? DexTokensSwapPairs { get; init; }

        /// <inheritdoc cref="BaseEvmWalletStats{TTransactionIntervalData}.AdjustingScoreMultipliers"/>
        [JsonIgnore]
        public override IEnumerable<Func<double>> AdjustingScoreMultipliers => base.AdjustingScoreMultipliers
            .Union(new List<Func<double>>
            {
                () => (this as IWalletHapiStats).GetAdjustingScoreMultiplier()
            });

        /// <inheritdoc cref="IWalletCommonStats{ITransactionIntervalData}.ExcludedStatDescriptions"/>
        [JsonIgnore]
        public override IEnumerable<string> ExcludedStatDescriptions =>
            base.ExcludedStatDescriptions
                .Union(new List<string>
                {
                    nameof(DexTokensSwapPairs),
                    nameof(HapiRiskScore)
                });
    }
}