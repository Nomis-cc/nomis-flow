// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountKIP17TokenEvent.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account KIP-17 token transfer event.
    /// </summary>
    public class KlaytnscopeAccountKIP17TokenEvent
    {
        /// <summary>
        /// Updated at.
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public BigInteger UpdatedAt { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Token address.
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }

        /// <summary>
        /// Token count.
        /// </summary>
        [JsonPropertyName("tokenCount")]
        public string? TokenCount { get; set; }
    }
}