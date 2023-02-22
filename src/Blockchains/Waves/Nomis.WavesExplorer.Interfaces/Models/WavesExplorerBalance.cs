// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerBalance.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.WavesExplorer.Interfaces.Models
{
    /// <summary>
    /// Waves Explorer balance.
    /// </summary>
    public class WavesExplorerBalance
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Regular balance.
        /// </summary>
        [JsonPropertyName("regular")]
        public BigInteger RegularBalance { get; set; }

        /// <summary>
        /// Generating balance.
        /// </summary>
        [JsonPropertyName("generating")]
        public BigInteger GeneratingBalance { get; set; }

        /// <summary>
        /// Available balance.
        /// </summary>
        [JsonPropertyName("available")]
        public BigInteger AvailableBalance { get; set; }

        /// <summary>
        /// Effective balance.
        /// </summary>
        [JsonPropertyName("effective")]
        public BigInteger EffectiveBalance { get; set; }
    }
}