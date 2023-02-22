// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanSplTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan transfer list.
    /// </summary>
    public class SolscanSplTransferList
        : ISolscanTransferList<SolscanSplTransfer>
    {
        /// <summary>
        /// Total amount of transfers.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }

        /// <summary>
        /// Spl transfer list.
        /// </summary>
        [JsonPropertyName("data")]
        public IList<SolscanSplTransfer> Data { get; set; } = new List<SolscanSplTransfer>();
    }
}