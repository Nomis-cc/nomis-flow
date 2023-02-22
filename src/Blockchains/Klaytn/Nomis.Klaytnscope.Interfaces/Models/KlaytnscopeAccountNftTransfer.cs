// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountNftTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account nft transfer.
    /// </summary>
    public class KlaytnscopeAccountNftTransfer :
        IKlaytnscopeTransfer
    {
        /// <summary>
        /// Created at.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public BigInteger CreatedAt { get; set; }

        /// <summary>
        /// Parent hash.
        /// </summary>
        [JsonPropertyName("parentHash")]
        public string? ParentHash { get; set; }

        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("blockNumber")]
        public int BlockNumber { get; set; }

        /// <summary>
        /// From address.
        /// </summary>
        [JsonPropertyName("fromAddress")]
        public string? FromAddress { get; set; }

        /// <summary>
        /// Token address.
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }

        /// <summary>
        /// To address.
        /// </summary>
        [JsonPropertyName("toAddress")]
        public string? ToAddress { get; set; }

        /// <summary>
        /// Token identifier.
        /// </summary>
        [JsonPropertyName("tokenId")]
        public string? TokenId { get; set; }

        /// <summary>
        /// Token count.
        /// </summary>
        [JsonPropertyName("tokenCount")]
        public string? TokenCount { get; set; }
    }
}