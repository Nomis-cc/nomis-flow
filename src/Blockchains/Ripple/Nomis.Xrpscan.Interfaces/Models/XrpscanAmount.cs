// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanAmount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan amount data.
    /// </summary>
    public class XrpscanAmount
    {
        /// <summary>
        /// Currency.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Issuer.
        /// </summary>
        [JsonPropertyName("issuer")]
        public string? Issuer { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [JsonPropertyName("value")]
        public object? Value { get; set; }
    }
}