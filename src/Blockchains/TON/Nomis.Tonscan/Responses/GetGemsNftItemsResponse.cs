// ------------------------------------------------------------------------------------------------------
// <copyright file="GetGemsNftItemsResponse.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Tonscan.Models;

namespace Nomis.Tonscan.Responses
{
    /// <summary>
    /// GetGems NFT items response.
    /// </summary>
    public class GetGemsNftItemsResponse
    {
        /// <summary>
        /// Cursor.
        /// </summary>
        [JsonPropertyName("cursor")]
        public string? Cursor { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        [JsonPropertyName("items")]
        public IList<GetGemsNftItem> Items { get; set; } = new List<GetGemsNftItem>();
    }
}