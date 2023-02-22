// ------------------------------------------------------------------------------------------------------
// <copyright file="SongbirdExplorerAccountNormalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.SongbirdExplorer.Interfaces.Models
{
    /// <summary>
    /// Songbird Explorer account normal transactions.
    /// </summary>
    public class SongbirdExplorerAccountNormalTransactions :
        ISongbirdExplorerTransferList<SongbirdExplorerAccountNormalTransaction>
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
        /// Account normal transaction list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<SongbirdExplorerAccountNormalTransaction> Data { get; set; } = new List<SongbirdExplorerAccountNormalTransaction>();
    }
}