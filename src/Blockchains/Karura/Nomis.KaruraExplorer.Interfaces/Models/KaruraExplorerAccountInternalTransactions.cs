// ------------------------------------------------------------------------------------------------------
// <copyright file="KaruraExplorerAccountInternalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.KaruraExplorer.Interfaces.Models
{
    /// <summary>
    /// Karura Explorer account internal transactions.
    /// </summary>
    public class KaruraExplorerAccountInternalTransactions :
        IKaruraExplorerTransferList<KaruraExplorerAccountInternalTransaction>
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
        public IList<KaruraExplorerAccountInternalTransaction> Data { get; set; } = new List<KaruraExplorerAccountInternalTransaction>();
    }
}