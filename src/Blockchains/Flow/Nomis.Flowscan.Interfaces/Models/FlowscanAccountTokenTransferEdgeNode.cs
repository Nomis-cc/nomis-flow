// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountTokenTransferEdgeNode.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan account token transfer edge node data.
    /// </summary>
    public class FlowscanAccountTokenTransferEdgeNode
    {
        /// <summary>
        /// Transaction.
        /// </summary>
        [JsonPropertyName("transaction")]
        public FlowscanTransaction? Transaction { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public FlowscanAmount? Amount { get; set; }

        /// <summary>
        /// Counterparty.
        /// </summary>
        [JsonPropertyName("counterparty")]
        public FlowscanAddress? Counterparty { get; set; }

        /// <summary>
        /// Counterparties count.
        /// </summary>
        [JsonPropertyName("counterpartiesCount")]
        public int? CounterpartiesCount { get; set; }
    }
}