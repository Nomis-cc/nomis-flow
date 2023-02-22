// ------------------------------------------------------------------------------------------------------
// <copyright file="CoinexCscUsdPriceResponse.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Responses
{
    /// <summary>
    /// Coinex CSC USD price response.
    /// </summary>
    public class CoinexCscUsdPriceResponse
    {
        /// <summary>
        /// Code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Prices dictionary.
        /// </summary>
        [JsonPropertyName("data")]
        public IDictionary<string, string>? Data { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}