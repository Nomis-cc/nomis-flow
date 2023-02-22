// ------------------------------------------------------------------------------------------------------
// <copyright file="CeloWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Dex.Abstractions.Contracts;
using Nomis.Dex.Abstractions.Stats;

namespace Nomis.Celoscan.Interfaces.Models
{
    /// <summary>
    /// Celo wallet stats.
    /// </summary>
    public sealed class CeloWalletStats :
        BaseEvmWalletStats<CeloTransactionIntervalData>,
        IWalletDexTokenSwapPairsStats,
        IWalletNftStats
    {
        /// <inheritdoc/>
        public override string NativeToken => "CELO";

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
        [Display(Description = "DEX tokens balances", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<DexTokenSwapPairsData>? DexTokensSwapPairs { get; init; }

        /// <inheritdoc cref="IWalletCommonStats{ITransactionIntervalData}.ExcludedStatDescriptions"/>
        [JsonIgnore]
        public override IEnumerable<string> ExcludedStatDescriptions =>
            base.ExcludedStatDescriptions
                .Union(new List<string>
                {
                    nameof(DexTokensSwapPairs)
                });
    }
}