// ------------------------------------------------------------------------------------------------------
// <copyright file="IAeternityExplorerTransferTx.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer transfer transaction.
    /// </summary>
    public interface IAeternityExplorerTransferTx
    {
        /// <summary>
        /// From address.
        /// </summary>
        [JsonPropertyName("sender_id")]
        public string? From { get; set; }

        /// <summary>
        /// To address.
        /// </summary>
        [JsonPropertyName("recipient_id")]
        public string? To { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [JsonPropertyName("amount")]
        public BigInteger Amount { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}