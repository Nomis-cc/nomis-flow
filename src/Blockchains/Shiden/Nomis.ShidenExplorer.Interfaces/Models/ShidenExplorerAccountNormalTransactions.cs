// ------------------------------------------------------------------------------------------------------
// <copyright file="ShidenExplorerAccountNormalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.ShidenExplorer.Interfaces.Models
{
    /// <summary>
    /// Shiden Explorer account normal transactions.
    /// </summary>
    public class ShidenExplorerAccountNormalTransactions :
        IShidenExplorerTransferList<ShidenExplorerAccountNormalTransaction>
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
        public IList<ShidenExplorerAccountNormalTransaction> Data { get; set; } = new List<ShidenExplorerAccountNormalTransaction>();
    }
}