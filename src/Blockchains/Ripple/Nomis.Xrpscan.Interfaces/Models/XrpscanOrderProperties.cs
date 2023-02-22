// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanOrderProperties.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan order properties.
    /// </summary>
    public class XrpscanOrderProperties
    {
        /// <summary>
        /// Maker.
        /// </summary>
        [JsonPropertyName("maker")]
        public string? Maker { get; set; }

        /// <summary>
        /// Sequence.
        /// </summary>
        [JsonPropertyName("sequence")]
        public ulong Sequence { get; set; }

        /// <summary>
        /// Maker exchange rate.
        /// </summary>
        [JsonPropertyName("makerExchangeRate")]
        public string? MakerExchangeRate { get; set; }
    }
}