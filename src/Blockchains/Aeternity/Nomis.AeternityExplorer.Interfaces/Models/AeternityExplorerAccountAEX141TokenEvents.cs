// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorerAccountAEX141TokenEvents.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer account AEX-141 token transfer events.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class AeternityExplorerAccountAEX141TokenEvents :
        IAeternityExplorerTransferList<AeternityExplorerAccountAEX141TokenEvent>
    {
        /// <summary>
        /// Account AEX-141 token event list.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public IList<AeternityExplorerAccountAEX141TokenEvent>? Data { get; set; }

        /// <summary>
        /// Next list of transfers.
        /// </summary>
        [JsonPropertyName("next")]
        public string? Next { get; set; }

        /// <summary>
        /// Previous list of transfers.
        /// </summary>
        [JsonPropertyName("prev")]
        public string? Prev { get; set; }
    }
}