// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerAccountAssetIssueTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.WavesExplorer.Interfaces.Models
{
    /// <summary>
    /// Waves Explorer account asset issue transaction data.
    /// </summary>
    public class WavesExplorerAccountAssetIssueTransaction
    {
        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }

        /// <summary>
        /// Id.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public ulong Timestamp { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        [JsonPropertyName("sender")]
        public string? Sender { get; set; }

        /// <summary>
        /// Sender public key.
        /// </summary>
        [JsonPropertyName("senderPublicKey")]
        public string? SenderPublicKey { get; set; }

        /// <summary>
        /// Signature.
        /// </summary>
        [JsonPropertyName("signature")]
        public string? Signature { get; set; }

        /// <summary>
        /// AssetId.
        /// </summary>
        [JsonPropertyName("assetId")]
        public string? AssetId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Quantity.
        /// </summary>
        [JsonPropertyName("quantity")]
        public ulong Quantity { get; set; }

        /// <summary>
        /// Reissuable.
        /// </summary>
        [JsonPropertyName("reissuable")]
        public bool Reissuable { get; set; }

        /// <summary>
        /// Decimals.
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}