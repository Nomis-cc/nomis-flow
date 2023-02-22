// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountTransactionEdge.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// FlowscanAccount transaction edge data.
    /// </summary>
    public class FlowscanAccountTransactionEdge
    {
        /// <summary>
        /// Transaction.
        /// </summary>
        [JsonPropertyName("node")]
        public FlowscanTransaction? Transaction { get; set; }
    }
}