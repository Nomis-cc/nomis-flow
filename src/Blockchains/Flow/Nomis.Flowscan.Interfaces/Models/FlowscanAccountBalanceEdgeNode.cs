// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountBalanceEdgeNode.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan account balance edge node data.
    /// </summary>
    public class FlowscanAccountBalanceEdgeNode
    {
        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public FlowscanAmount? Amount { get; set; }
    }
}