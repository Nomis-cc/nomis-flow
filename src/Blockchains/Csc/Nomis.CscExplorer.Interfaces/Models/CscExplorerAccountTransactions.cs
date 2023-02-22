// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerAccountTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer transactions.
    /// </summary>
    public class CscExplorerAccountTransactions :
        ICscExplorerTransferList<CscExplorerAccountTransactionData, CscExplorerAccountTransactionRecord>
    {
        /// <summary>
        /// Code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Transactions data.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public CscExplorerAccountTransactionData? Data { get; set; }
    }
}