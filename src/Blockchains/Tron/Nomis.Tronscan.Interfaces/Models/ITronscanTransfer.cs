// ------------------------------------------------------------------------------------------------------
// <copyright file="ITronscanTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan transfer.
    /// </summary>
    public interface ITronscanTransfer
    {
        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("block")]
        public long Block { get; set; }
    }
}