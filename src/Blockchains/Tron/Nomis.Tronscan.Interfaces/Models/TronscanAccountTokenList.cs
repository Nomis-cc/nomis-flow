// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountTokenList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account token list.
    /// </summary>
    public class TronscanAccountTokenList
    {
        /// <summary>
        /// The token id.
        /// </summary>
        [JsonPropertyName("tokenId")]
        public string? TokenId { get; set; }

        /// <summary>
        /// Call value.
        /// </summary>
        [JsonPropertyName("call_value")]
        public long CallValue { get; set; }

        /// <summary>
        /// Token info.
        /// </summary>
        [JsonPropertyName("tokenInfo")]
        public TronscanAccountTokenInfo? TokenInfo { get; set; }
    }
}