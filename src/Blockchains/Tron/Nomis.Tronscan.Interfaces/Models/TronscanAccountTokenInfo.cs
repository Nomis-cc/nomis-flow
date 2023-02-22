// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountTokenInfo.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account token info data.
    /// </summary>
    public class TronscanAccountTokenInfo
    {
        /// <summary>
        /// The token id.
        /// </summary>
        [JsonPropertyName("tokenId")]
        public string? TokenId { get; set; }

        /// <summary>
        /// The token abbr.
        /// </summary>
        [JsonPropertyName("tokenAbbr")]
        public string? TokenAbbr { get; set; }

        /// <summary>
        /// The token name.
        /// </summary>
        [JsonPropertyName("tokenName")]
        public string? TokenName { get; set; }

        /// <summary>
        /// The token decimal.
        /// </summary>
        [JsonPropertyName("tokenDecimal")]
        public int TokenDecimal { get; set; }

        /// <summary>
        /// The token can show.
        /// </summary>
        [JsonPropertyName("tokenCanShow")]
        public int TokenCanShow { get; set; }

        /// <summary>
        /// The token type.
        /// </summary>
        [JsonPropertyName("tokenType")]
        public string? TokenType { get; set; }

        /// <summary>
        /// The token logo.
        /// </summary>
        [JsonPropertyName("tokenLogo")]
        public string? TokenLogo { get; set; }

        /// <summary>
        /// The token level.
        /// </summary>
        [JsonPropertyName("tokenLevel")]
        public string? TokenLevel { get; set; }

        /// <summary>
        /// VIP.
        /// </summary>
        [JsonPropertyName("vip")]
        public bool Vip { get; set; }
    }
}