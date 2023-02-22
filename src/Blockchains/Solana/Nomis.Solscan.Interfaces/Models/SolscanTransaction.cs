// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Utils.Extensions;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan transaction.
    /// </summary>
    public class SolscanTransaction
    {
        /// <summary>
        /// Lamport.
        /// </summary>
        [JsonPropertyName("lamport")]
        public long Lamport { get; set; }

        /// <summary>
        /// Transaction hash.
        /// </summary>
        [JsonPropertyName("txHash")]
        public string? TxHash { get; set; }

        /// <summary>
        /// The block time.
        /// </summary>
        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        /// <summary>
        /// The block time.
        /// </summary>
        public DateTime Date => BlockTime.ToString().ToDateTime();

        /// <summary>
        /// Transaction status.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Transaction parsed instruction list.
        /// </summary>
        [JsonPropertyName("parsedInstruction")]
        public IList<SolscanTransactionParsedInstruction> ParsedInstructions { get; set; } = new List<SolscanTransactionParsedInstruction>();
    }
}