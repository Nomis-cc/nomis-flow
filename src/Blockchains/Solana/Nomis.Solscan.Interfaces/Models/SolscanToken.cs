// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanToken.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan token.
    /// </summary>
    public class SolscanToken
    {
        /// <summary>
        /// Token address.
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }

        /// <summary>
        /// Token amount.
        /// </summary>
        [JsonPropertyName("tokenAmount")]
        public SolscanTokenAmount? TokenAmount { get; set; }

        /// <summary>
        /// Token symbol.
        /// </summary>
        [JsonPropertyName("tokenSymbol")]
        public string? TokenSymbol { get; set; }
    }
}