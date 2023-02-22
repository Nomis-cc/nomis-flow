// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountTokenBalanceData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account token balance data.
    /// </summary>
    public class KlaytnscopeAccountTokenBalanceData
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Token aAddress.
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }
    }
}