// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerAccountAsset.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.WavesExplorer.Interfaces.Models
{
    /// <summary>
    /// Waves Explorer account asset data.
    /// </summary>
    public class WavesExplorerAccountAsset
    {
        /// <summary>
        /// Asset Id.
        /// </summary>
        [JsonPropertyName("assetId")]
        public string? AssetId { get; set; }

        /// <summary>
        /// Reissuable.
        /// </summary>
        [JsonPropertyName("reissuable")]
        public bool Reissuable { get; set; }

        /// <summary>
        /// Sponsor balance.
        /// </summary>
        [JsonPropertyName("sponsorBalance")]
        public ulong? SponsorBalance { get; set; }

        /// <summary>
        /// Quantity.
        /// </summary>
        [JsonPropertyName("quantity")]
        public ulong Quantity { get; set; }

        /// <summary>
        /// Issue transaction.
        /// </summary>
        [JsonPropertyName("issueTransaction")]
        public WavesExplorerAccountAssetIssueTransaction? IssueTransaction { get; set; }

        /// <summary>
        /// Balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public ulong Balance { get; set; }
    }
}