// ------------------------------------------------------------------------------------------------------
// <copyright file="HaqqExplorerAccountInternalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.HaqqExplorer.Interfaces.Models
{
    /// <summary>
    /// HAQQ Explorer account internal transactions.
    /// </summary>
    public class HaqqExplorerAccountInternalTransactions :
        IHaqqExplorerTransferList<HaqqExplorerAccountInternalTransaction>
    {
        /// <summary>
        /// Status.
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Account internal transaction list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<HaqqExplorerAccountInternalTransaction> Data { get; set; } = new List<HaqqExplorerAccountInternalTransaction>();
    }
}