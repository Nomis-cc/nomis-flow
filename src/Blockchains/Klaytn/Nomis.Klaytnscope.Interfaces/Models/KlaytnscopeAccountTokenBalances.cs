// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountTokenBalances.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account token balances data.
    /// </summary>
    public class KlaytnscopeAccountTokenBalances
    {
        /// <summary>
        /// Success.
        /// </summary>
        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        /// <summary>
        /// Code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Account token balance data.
        /// </summary>
        [JsonPropertyName("result")]
        public IList<KlaytnscopeAccountTokenBalanceData> Result { get; set; } = new List<KlaytnscopeAccountTokenBalanceData>();
    }
}