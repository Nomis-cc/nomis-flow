// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanAccount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Interfaces.Models
{
    /// <summary>
    /// Tonscan account data.
    /// </summary>
    public class TonscanAccount
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Icon.
        /// </summary>
        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Is scam.
        /// </summary>
        [JsonPropertyName("is_scam")]
        public bool IsScam { get; set; }
    }
}