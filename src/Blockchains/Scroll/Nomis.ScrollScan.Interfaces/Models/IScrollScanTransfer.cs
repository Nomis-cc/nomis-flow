// ------------------------------------------------------------------------------------------------------
// <copyright file="IScrollScanTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.ScrollScan.Interfaces.Models
{
    /// <summary>
    /// ScrollScan transfer.
    /// </summary>
    public interface IScrollScanTransfer
    {
        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("blockNumber")]
        public string? BlockNumber { get; set; }
    }
}