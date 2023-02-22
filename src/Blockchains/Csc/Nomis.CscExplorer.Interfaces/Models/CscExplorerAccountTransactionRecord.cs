// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerAccountTransactionRecord.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer transaction record.
    /// </summary>
    public class CscExplorerAccountTransactionRecord :
        ICscExplorerTransferRecord
    {
        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

        /// <summary>
        /// Fee.
        /// </summary>
        [JsonPropertyName("fee")]
        public string? Fee { get; set; }

        /// <summary>
        /// Error.
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        /// <summary>
        /// From.
        /// </summary>
        [JsonPropertyName("from")]
        public CscExplorerAccountAddress? From { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        [JsonPropertyName("height")]
        public long Height { get; set; }

        /// <summary>
        /// Index.
        /// </summary>
        [JsonPropertyName("index")]
        public long Index { get; set; }

        /// <summary>
        /// Method.
        /// </summary>
        [JsonPropertyName("method")]
        public string? Method { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        [JsonPropertyName("status")]
        public long Status { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }

        /// <summary>
        /// To.
        /// </summary>
        [JsonPropertyName("to")]
        public CscExplorerAccountAddress? To { get; set; }

        /// <summary>
        /// Transaction hash.
        /// </summary>
        [JsonPropertyName("tx_hash")]
        public string? TxHash { get; set; }
    }
}