// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanAccountAssetBalances.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Interfaces.Models
{
    /// <summary>
    /// Tonscan account asset balances data.
    /// </summary>
    public class TonscanAccountAssetBalances
    {
        /// <summary>
        /// Asset balances.
        /// </summary>
        [JsonPropertyName("balances")]
        public IList<TonscanAccountAssetBalance> Balances { get; set; } = new List<TonscanAccountAssetBalance>();
    }
}