// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountContracts.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account contracts data.
    /// </summary>
    public class TronscanAccountContracts
    {
        /// <summary>
        /// Total count.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }

        /// <summary>
        /// The list of contracts.
        /// </summary>
        [JsonPropertyName("data")]
        public IList<TronscanAccountContract>? Data { get; set; } = new List<TronscanAccountContract>();
    }
}