// ------------------------------------------------------------------------------------------------------
// <copyright file="GetGemsNftItemSale.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Models
{
    /// <summary>
    /// GetGems NFT item sale data.
    /// </summary>
    public class GetGemsNftItemSale
    {
        /// <summary>
        /// Full price.
        /// </summary>
        [JsonPropertyName("fullPrice")]
        public string? FullPrice { get; set; }

        /// <summary>
        /// NFT owner address.
        /// </summary>
        [JsonPropertyName("nftOwnerAddress")]
        public string? NftOwnerAddress { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }
}