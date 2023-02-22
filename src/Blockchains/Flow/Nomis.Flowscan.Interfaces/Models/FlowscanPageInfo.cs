// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanPageInfo.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Flowscan.Interfaces.Models
{
    /// <summary>
    /// Flowscan page info data.
    /// </summary>
    public class FlowscanPageInfo
    {
        /// <summary>
        /// Has next page.
        /// </summary>
        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }

        /// <summary>
        /// End cursor.
        /// </summary>
        [JsonPropertyName("endCursor")]
        public string? EndCursor { get; set; }
    }
}