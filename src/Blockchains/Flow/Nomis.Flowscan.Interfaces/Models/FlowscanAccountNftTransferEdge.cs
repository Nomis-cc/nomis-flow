// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountNftTransferEdge.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan account NFT transfer edge data.
    /// </summary>
    public class FlowscanAccountNftTransferEdge
    {
        /// <summary>
        /// Node.
        /// </summary>
        [JsonPropertyName("node")]
        public FlowscanAccountNftTransferEdgeNode? Node { get; set; }
    }
}