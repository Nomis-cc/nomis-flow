// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerAccountNftAsset.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.WavesExplorer.Interfaces.Models
{
    /// <summary>
    /// Waves Explorer account NFT asset data.
    /// </summary>
    public class WavesExplorerAccountNftAsset
    {
        /// <summary>
        /// Asset Id.
        /// </summary>
        [JsonPropertyName("assetId")]
        public string? AssetId { get; set; }

        /// <summary>
        /// Issue height.
        /// </summary>
        [JsonPropertyName("issueHeight")]
        public ulong IssueHeight { get; set; }

        /// <summary>
        /// Issue timestamp.
        /// </summary>
        [JsonPropertyName("issueTimestamp")]
        public ulong IssueTimestamp { get; set; }

        /// <summary>
        /// Issuer.
        /// </summary>
        [JsonPropertyName("issuer")]
        public string? Issuer { get; set; }

        /// <summary>
        /// Issuer public key.
        /// </summary>
        [JsonPropertyName("issuerPublicKey")]
        public string? IssuerPublicKey { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Decimals.
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        /// <summary>
        /// Reissuable.
        /// </summary>
        [JsonPropertyName("reissuable")]
        public bool Reissuable { get; set; }

        /// <summary>
        /// Quantity.
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Scripted.
        /// </summary>
        [JsonPropertyName("scripted")]
        public bool Scripted { get; set; }

        /// <summary>
        /// Origin transaction Id.
        /// </summary>
        [JsonPropertyName("originTransactionId")]
        public string? OriginTransactionId { get; set; }
    }
}