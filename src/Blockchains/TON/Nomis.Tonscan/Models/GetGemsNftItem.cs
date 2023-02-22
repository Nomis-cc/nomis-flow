// ------------------------------------------------------------------------------------------------------
// <copyright file="GetGemsNftItem.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Models
{
    /// <summary>
    /// GetGems NFT item data.
    /// </summary>
    public class GetGemsNftItem
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Kind.
        /// </summary>
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        /// <summary>
        /// Collection.
        /// </summary>
        [JsonPropertyName("collection")]
        public GetGemsNftItemCollection? Collection { get; set; }

        /// <summary>
        /// Sale data.
        /// </summary>
        [JsonPropertyName("sale")]
        public GetGemsNftItemSale? Sale { get; set; }
    }
}