// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorerAccountNormalTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer account normal transaction.
    /// </summary>
    public class AeternityExplorerAccountNormalTransaction :
        IAeternityExplorerTransfer
    {
        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("block_height")]
        public long BlockNumber { get; set; }

        /// <summary>
        /// Block hash.
        /// </summary>
        [JsonPropertyName("block_hash")]
        public string? BlockHash { get; set; }

        /// <summary>
        /// Time stamp.
        /// </summary>
        [JsonPropertyName("micro_time")]
        public long MicroTime { get; set; }

        /// <summary>
        /// Hash.
        /// </summary>
        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("tx")]
        public AeternityExplorerTransferTx? Transaction { get; set; }
    }
}