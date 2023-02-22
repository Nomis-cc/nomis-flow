// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerTokensDataItem.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer account tokens data item.
    /// </summary>
    public class CscExplorerTokensDataItem
    {
        /// <summary>
        /// Balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public string? Balance { get; set; }

        /// <summary>
        /// Token info.
        /// </summary>
        [JsonPropertyName("token_info")]
        public CscExplorerTokensDataItemTokenInfo? TokenInfo { get; set; }
    }
}