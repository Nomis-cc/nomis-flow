// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.WavesExplorer.Interfaces.Models
{
    /// <summary>
    /// Waves Explorer transaction data.
    /// </summary>
    public class WavesExplorerTransaction
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
        /// Recipient.
        /// </summary>
        [JsonPropertyName("recipient")]
        public string? Recipient { get; set; }

        /// <summary>
        /// AssetId.
        /// </summary>
        [JsonPropertyName("assetId")]
        public string? AssetId { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public ulong Amount { get; set; }

        /// <summary>
        /// Attachment.
        /// </summary>
        [JsonPropertyName("attachment")]
        public string? Attachment { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        [JsonPropertyName("height")]
        public ulong Height { get; set; }

        /// <summary>
        /// Application status.
        /// </summary>
        [JsonPropertyName("applicationStatus")]
        public string? ApplicationStatus { get; set; }
    }
}