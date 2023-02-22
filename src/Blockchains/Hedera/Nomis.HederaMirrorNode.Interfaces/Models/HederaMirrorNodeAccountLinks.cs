// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeAccountLinks.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node account links.
    /// </summary>
    public class HederaMirrorNodeAccountLinks
    {
        /// <summary>
        /// Hyperlink to the next page of results.
        /// </summary>
        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }
}