// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorerAccountInternalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer account internal transactions.
    /// </summary>
    public class AeternityExplorerAccountInternalTransactions :
        IAeternityExplorerTransferList<AeternityExplorerAccountInternalTransaction>
    {
        /// <summary>
        /// Account internal transaction list.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public IList<AeternityExplorerAccountInternalTransaction>? Data { get; set; }

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