// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanCounterpartyName.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan counterparty name.
    /// </summary>
    public class XrpscanCounterpartyName
    {
        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [JsonPropertyName("desc")]
        public string? Description { get; set; }

        /// <summary>
        /// Account.
        /// </summary>
        [JsonPropertyName("account")]
        public string? Account { get; set; }

        /// <summary>
        /// Domain.
        /// </summary>
        [JsonPropertyName("domain")]
        public string? Domain { get; set; }

        /// <summary>
        /// Twitter.
        /// </summary>
        [JsonPropertyName("twitter")]
        public string? Twitter { get; set; }
    }
}