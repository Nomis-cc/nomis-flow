// ------------------------------------------------------------------------------------------------------
// <copyright file="GetGemsNftItemCollection.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Models
{
    /// <summary>
    /// GetGems NFT item collection data.
    /// </summary>
    public class GetGemsNftItemCollection
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Is verified.
        /// </summary>
        [JsonPropertyName("isVerified")]
        public bool IsVerified { get; set; }
    }
}