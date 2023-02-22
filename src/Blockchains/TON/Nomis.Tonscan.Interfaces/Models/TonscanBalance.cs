// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanBalance.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Interfaces.Models
{
    /// <summary>
    /// Tonscan account.
    /// </summary>
    public class TonscanBalance
    {
        /// <summary>
        /// Status.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public BigInteger Balance { get; set; }
    }
}