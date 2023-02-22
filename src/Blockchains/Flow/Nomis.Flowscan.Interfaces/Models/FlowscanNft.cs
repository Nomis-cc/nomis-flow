// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanNft.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan NFT data.
    /// </summary>
    public class FlowscanNft
    {
        /// <summary>
        /// Contract.
        /// </summary>
        [JsonPropertyName("contract")]
        public FlowscanContract? Contract { get; set; }

        /// <summary>
        /// NFT id.
        /// </summary>
        [JsonPropertyName("nftId")]
        public string? NftId { get; set; }
    }
}