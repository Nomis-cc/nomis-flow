// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanAmount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan amount.
    /// </summary>
    public class FlowscanAmount
    {
        /// <summary>
        /// Token.
        /// </summary>
        [JsonPropertyName("token")]
        public FlowscanToken? Token { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// USD value.
        /// </summary>
        [JsonPropertyName("usdValue")]
        public decimal? UsdValue { get; set; }
    }
}