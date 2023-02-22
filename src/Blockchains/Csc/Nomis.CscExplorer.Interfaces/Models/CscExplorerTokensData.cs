// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerTokensData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

// ReSharper disable UnusedMember.Global
namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer account tokens data.
    /// </summary>
    public class CscExplorerTokensData
    {
        /// <summary>
        /// CRC20 tokens list.
        /// </summary>
        [JsonPropertyName("crc20")]
        public IList<CscExplorerTokensDataItem> Crc20List { get; set; } = new List<CscExplorerTokensDataItem>();

        /// <summary>
        /// CRC721 tokens list.
        /// </summary>
        [JsonPropertyName("crc721")]
        public IList<CscExplorerTokensDataItem> Crc721List { get; set; } = new List<CscExplorerTokensDataItem>();
    }
}