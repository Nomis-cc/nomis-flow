// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorerAccountAEX141TokenEvent.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer account AEX-141 token transfer event.
    /// </summary>
    public class AeternityExplorerAccountAEX141TokenEvent
    {
        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("block_height")]
        public long BlockHeight { get; set; }

        /// <summary>
        /// Call transaction identifier.
        /// </summary>
        [JsonPropertyName("call_txi")]
        public long CallTxi { get; set; }

        /// <summary>
        /// Contract identifier.
        /// </summary>
        [JsonPropertyName("contract_id")]
        public string? ContractId { get; set; }

        /// <summary>
        /// Micro time.
        /// </summary>
        [JsonPropertyName("micro_time")]
        public long MicroTime { get; set; }

        /// <summary>
        /// Recipient.
        /// </summary>
        [JsonPropertyName("recipient")]
        public string? Recipient { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        [JsonPropertyName("sender")]
        public string? Sender { get; set; }

        /// <summary>
        /// Token identifier.
        /// </summary>
        [JsonPropertyName("token_id")]
        public long TokenId { get; set; }

        /// <summary>
        /// Transaction hash.
        /// </summary>
        [JsonPropertyName("tx_hash")]
        public string? TxHash { get; set; }
    }
}