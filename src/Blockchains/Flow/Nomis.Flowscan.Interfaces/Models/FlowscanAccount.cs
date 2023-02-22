// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan account data.
    /// </summary>
    public class FlowscanAccount :
        FlowscanAddress
    {
        /// <summary>
        /// Contracts.
        /// </summary>
        [JsonPropertyName("contracts")]
        public IList<FlowscanAccountContract> Contracts { get; set; } = new List<FlowscanAccountContract>();

        /// <summary>
        /// Token balances.
        /// </summary>
        [JsonPropertyName("tokenBalances")]
        public FlowscanAccountBalances? TokenBalances { get; set; }
    }
}