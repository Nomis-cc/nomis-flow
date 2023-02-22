// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeAccountKey.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node account key data.
    /// </summary>
    public class HederaMirrorNodeAccountKey
    {
        /// <summary>
        /// The type of key.
        /// </summary>
        [JsonPropertyName("_type")]
        public string? Type { get; set; }

        /// <summary>
        /// The public key.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }
    }
}