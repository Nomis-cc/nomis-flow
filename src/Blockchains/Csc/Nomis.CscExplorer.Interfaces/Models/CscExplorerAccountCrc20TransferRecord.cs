// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerAccountCrc20TransferRecord.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer CRC-20 transfer record.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class CscExplorerAccountCrc20TransferRecord :
        ICscExplorerTransferRecord
    {
        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

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

        /// <summary>
        /// Token info.
        /// </summary>
        [JsonPropertyName("token_info")]
        public CscExplorerAccountTransferRecordTokenInfo? TokenInfo { get; set; }
    }
}