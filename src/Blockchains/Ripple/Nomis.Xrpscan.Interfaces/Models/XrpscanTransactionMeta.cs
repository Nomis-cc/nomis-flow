// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanTransactionMeta.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan transaction meta.
    /// </summary>
    public class XrpscanTransactionMeta
    {
        /// <summary>
        /// Transaction index.
        /// </summary>
        [JsonPropertyName("TransactionIndex")]
        public long TransactionIndex { get; set; }

        /// <summary>
        /// Transaction result.
        /// </summary>
        [JsonPropertyName("TransactionResult")]
        public string? TransactionResult { get; set; }

        /// <summary>
        /// Delivered amount.
        /// </summary>
        [JsonPropertyName("delivered_amount")]
        public XrpscanAmount? DeliveredAmount { get; set; }
    }
}