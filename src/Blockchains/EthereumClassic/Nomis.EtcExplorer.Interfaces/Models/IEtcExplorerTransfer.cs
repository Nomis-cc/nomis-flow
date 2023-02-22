// ------------------------------------------------------------------------------------------------------
// <copyright file="IEtcExplorerTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.EtcExplorer.Interfaces.Models
{
    /// <summary>
    /// ETC Explorer transfer.
    /// </summary>
    public interface IEtcExplorerTransfer
    {
        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("blockNumber")]
        public string? BlockNumber { get; set; }
    }
}