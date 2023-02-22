// ------------------------------------------------------------------------------------------------------
// <copyright file="IAeternityExplorerTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer transfer.
    /// </summary>
    public interface IAeternityExplorerTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("tx")]
        public AeternityExplorerTransferTx? Transaction { get; set; }
    }
}