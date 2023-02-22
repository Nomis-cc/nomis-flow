// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerAccountTransactionData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer transaction data.
    /// </summary>
    public class CscExplorerAccountTransactionData :
        ICscExplorerTransferData<CscExplorerAccountTransactionRecord>
    {
        /// <summary>
        /// Has next page.
        /// </summary>
        [JsonPropertyName("has_next")]
        public bool HasNext { get; set; }

        /// <summary>
        /// Limit.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Page.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// Total.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [JsonPropertyName("records")]
        public IList<CscExplorerAccountTransactionRecord> Records { get; set; } = new List<CscExplorerAccountTransactionRecord>();
    }
}