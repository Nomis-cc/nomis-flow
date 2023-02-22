// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountNftTransfers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan account NFT transfers data.
    /// </summary>
    public class FlowscanAccountNftTransfers
    {
        /// <summary>
        /// Page info.
        /// </summary>
        [JsonPropertyName("pageInfo")]
        public FlowscanPageInfo? PageInfo { get; set; }

        /// <summary>
        /// Edges.
        /// </summary>
        [JsonPropertyName("edges")]
        public IList<FlowscanAccountNftTransferEdge> Edges { get; set; } = new List<FlowscanAccountNftTransferEdge>();
    }
}