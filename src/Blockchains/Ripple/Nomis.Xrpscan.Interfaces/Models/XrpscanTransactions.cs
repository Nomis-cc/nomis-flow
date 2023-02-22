// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan transactions.
    /// </summary>
    public class XrpscanTransactions
    {
        /// <summary>
        /// Account.
        /// </summary>
        [JsonPropertyName("account")]
        public string? Account { get; set; }

        /// <summary>
        /// Limit.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Marker.
        /// </summary>
        [JsonPropertyName("marker")]
        public string? Marker { get; set; }

        /// <summary>
        /// Transactions.
        /// </summary>
        [JsonPropertyName("transactions")]
        public IList<XrpscanTransaction> Transactions { get; set; } = new List<XrpscanTransaction>();
    }
}