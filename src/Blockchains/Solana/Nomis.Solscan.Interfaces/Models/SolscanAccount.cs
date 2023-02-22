// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanAccount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan account.
    /// </summary>
    public class SolscanAccount
    {
        /// <summary>
        /// Lamports.
        /// </summary>
        [JsonPropertyName("lamports")]
        public long Lamports { get; set; }
    }
}