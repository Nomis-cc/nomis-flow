// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanSolTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan sol transfer list.
    /// </summary>
    public class SolscanSolTransferList
        : ISolscanTransferList<SolscanSolTransfer>
    {
        /// <summary>
        /// Sol transfer list.
        /// </summary>
        [JsonPropertyName("data")]
        public IList<SolscanSolTransfer> Data { get; set; } = new List<SolscanSolTransfer>();
    }
}