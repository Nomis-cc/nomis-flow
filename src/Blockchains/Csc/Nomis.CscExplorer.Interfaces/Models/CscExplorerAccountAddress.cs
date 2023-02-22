// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerAccountAddress.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer account address.
    /// </summary>
    public class CscExplorerAccountAddress
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Alias.
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }
    }
}