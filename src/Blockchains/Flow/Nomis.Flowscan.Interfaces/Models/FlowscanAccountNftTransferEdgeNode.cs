// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAccountNftTransferEdgeNode.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan account NFT transfer edge node.
    /// </summary>
    public class FlowscanAccountNftTransferEdgeNode
    {
        /// <summary>
        /// Transaction.
        /// </summary>
        [JsonPropertyName("transaction")]
        public FlowscanTransaction? Transaction { get; set; }

        /// <summary>
        /// NFT.
        /// </summary>
        [JsonPropertyName("nft")]
        public FlowscanNft? Nft { get; set; }

        /// <summary>
        /// From.
        /// </summary>
        [JsonPropertyName("from")]
        public FlowscanAddress? From { get; set; }

        /// <summary>
        /// To.
        /// </summary>
        [JsonPropertyName("to")]
        public FlowscanAddress? To { get; set; }
    }
}