﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="BttcscanAccountInternalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Bttcscan.Interfaces.Models
{
    /// <summary>
    /// Bttcscan account internal transactions.
    /// </summary>
    public class BttcscanAccountInternalTransactions :
        IBttcscanTransferList<BttcscanAccountInternalTransaction>
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
        public IList<BttcscanAccountInternalTransaction> Data { get; set; } = new List<BttcscanAccountInternalTransaction>();
    }
}