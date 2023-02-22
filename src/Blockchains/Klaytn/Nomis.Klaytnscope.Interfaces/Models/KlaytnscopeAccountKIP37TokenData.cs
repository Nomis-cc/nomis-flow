// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountKIP37TokenData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account KIP-37 token data.
    /// </summary>
    public class KlaytnscopeAccountKIP37TokenData
    {
        /// <summary>
        /// Image.
        /// </summary>
        [JsonPropertyName("image")]
        public string? Image { get; set; }

        /// <summary>
        /// Token name.
        /// </summary>
        [JsonPropertyName("tokenName")]
        public string? TokenName { get; set; }

        /// <summary>
        /// Token symbol.
        /// </summary>
        [JsonPropertyName("tokenSymbol")]
        public string? TokenSymbol { get; set; }

        /// <summary>
        /// Whitelist flag.
        /// </summary>
        [JsonPropertyName("whitelistFlag")]
        public bool WhitelistFlag { get; set; }
    }
}