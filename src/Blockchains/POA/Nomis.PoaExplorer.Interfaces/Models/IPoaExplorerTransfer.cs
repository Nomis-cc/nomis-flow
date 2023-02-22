// ------------------------------------------------------------------------------------------------------
// <copyright file="IPoaExplorerTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.PoaExplorer.Interfaces.Models
{
    /// <summary>
    /// POA Explorer transfer.
    /// </summary>
    public interface IPoaExplorerTransfer
    {
        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("blockNumber")]
        public string? BlockNumber { get; set; }
    }
}