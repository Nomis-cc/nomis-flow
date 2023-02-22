// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountBalanceEdge.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan accountBalanceEdge
    /// </summary>
    public class FlowscanAccountBalanceEdge
    {
        /// <summary>
        /// Node.
        /// </summary>
        [JsonPropertyName("node")]
        public FlowscanAccountBalanceEdgeNode? Node { get; set; }
    }
}