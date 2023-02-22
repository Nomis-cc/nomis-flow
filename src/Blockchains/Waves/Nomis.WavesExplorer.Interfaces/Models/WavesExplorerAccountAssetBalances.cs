// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerAccountAssetBalances.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.WavesExplorer.Interfaces.Models
{
    /// <summary>
    /// Waves Explorer account asset balances data.
    /// </summary>
    public class WavesExplorerAccountAssetBalances
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Balances.
        /// </summary>
        [JsonPropertyName("balances")]
        public IList<WavesExplorerAccountAsset> Balances { get; set; } = new List<WavesExplorerAccountAsset>();
    }
}