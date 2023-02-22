// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanAccount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan account.
    /// </summary>
    public class XrpscanAccount
    {
        /// <summary>
        /// XRP balance.
        /// </summary>
        [JsonPropertyName("xrpBalance")]
        public string? XrpBalance { get; set; }

        /// <summary>
        /// Inception.
        /// </summary>
        [JsonPropertyName("inception")]
        public DateTime Inception { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("Address")]
        public string? Address { get; set; }
    }
}