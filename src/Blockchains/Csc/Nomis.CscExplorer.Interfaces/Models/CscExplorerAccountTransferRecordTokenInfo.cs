// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerAccountTransferRecordTokenInfo.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer transfer record token info.
    /// </summary>
    public class CscExplorerAccountTransferRecordTokenInfo
    {
        /// <summary>
        /// Contract.
        /// </summary>
        [JsonPropertyName("contract")]
        public string? Contract { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
    }
}