// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountNftTokenData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account NFT token data.
    /// </summary>
    public class KlaytnscopeAccountNftTokenData
    {
        /// <summary>
        /// Created at.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public BigInteger CreatedAt { get; set; }

        /// <summary>
        /// Token address.
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }

        /// <summary>
        /// Token name.
        /// </summary>
        [JsonPropertyName("tokenName")]
        public string? TokenName { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        /// <summary>
        /// Token type.
        /// </summary>
        [JsonPropertyName("tokenType")]
        public int TokenType { get; set; }

        /// <summary>
        /// Total supply.
        /// </summary>
        [JsonPropertyName("totalSupply")]
        public string? TotalSupply { get; set; }

        /// <summary>
        /// Total transfers.
        /// </summary>
        [JsonPropertyName("totalTransfers")]
        public int TotalTransfers { get; set; }

        /// <summary>
        /// Whitelist flag.
        /// </summary>
        [JsonPropertyName("whitelistFlag")]
        public bool WhitelistFlag { get; set; }
    }
}