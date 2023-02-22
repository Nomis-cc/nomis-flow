// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanSolTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Utils.Extensions;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan sol transfer.
    /// </summary>
    public class SolscanSolTransfer
        : ISolscanTransfer
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        /// <summary>
        /// Source address.
        /// </summary>
        [JsonPropertyName("src")]
        public string? Src { get; set; }

        /// <summary>
        /// Destination address.
        /// </summary>
        [JsonPropertyName("dst")]
        public string? Dst { get; set; }

        /// <summary>
        /// Transaction hash.
        /// </summary>
        [JsonPropertyName("txHash")]
        public string? TxHash { get; set; }

        /// <summary>
        /// Lamport.
        /// </summary>
        [JsonPropertyName("lamport")]
        public long Lamport { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Decimals.
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        /// <summary>
        /// The block time.
        /// </summary>
        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        /// <summary>
        /// The block time.
        /// </summary>
        public DateTime Date => BlockTime.ToString().ToDateTime();
    }
}