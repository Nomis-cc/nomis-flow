// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountBalances.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan account balances data.
    /// </summary>
    public class FlowscanAccountBalances
    {
        /// <summary>
        /// Edges.
        /// </summary>
        [JsonPropertyName("edges")]
        public IList<FlowscanAccountBalanceEdge> Edges { get; set; } = new List<FlowscanAccountBalanceEdge>();
    }
}