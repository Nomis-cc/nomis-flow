// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountNormalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account normal transactions.
    /// </summary>
    public class TronscanAccountNormalTransactions :
        ITronscanTransferList<TronscanAccountNormalTransaction>
    {
        /// <summary>
        /// Total transaction count.
        /// </summary>
        [JsonPropertyName("rangeTotal")]
        public long RangeTotal { get; set; }

        /// <summary>
        /// Total transaction count for this page.
        /// </summary>
        [JsonPropertyName("total")]
        public long Total { get; set; }

        /// <summary>
        /// List of transactions.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TronscanAccountNormalTransaction>? Data { get; set; } = new List<TronscanAccountNormalTransaction>();
    }
}