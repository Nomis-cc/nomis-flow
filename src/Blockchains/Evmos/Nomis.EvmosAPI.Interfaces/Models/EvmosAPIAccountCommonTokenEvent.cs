// ------------------------------------------------------------------------------------------------------
// <copyright file="EvmosAPIAccountCommonTokenEvent.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.EvmosAPI.Interfaces.Models
{
    /// <summary>
    /// EvmosAPI account common token transfer event.
    /// </summary>
    public class EvmosAPIAccountCommonTokenEvent
    {
        /// <summary>
        /// Balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public string? Balance { get; set; }

        /// <summary>
        /// Contract address.
        /// </summary>
        [JsonPropertyName("contractAddress")]
        public string? ContractAddress { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        /// <summary>
        /// Decimals.
        /// </summary>
        [JsonPropertyName("decimals")]
        public string? Decimals { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}