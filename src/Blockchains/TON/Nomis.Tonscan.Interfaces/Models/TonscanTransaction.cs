// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Interfaces.Models
{
    /// <summary>
    /// Tonscan transaction data.
    /// </summary>
    public class TonscanTransaction
    {
        /// <summary>
        /// Universal time.
        /// </summary>
        [JsonPropertyName("utime")]
        public BigInteger Utime { get; set; }

        /// <summary>
        /// Last transaction.
        /// </summary>
        [JsonPropertyName("lt")]
        public BigInteger Lt { get; set; }

        /// <summary>
        /// Hash.
        /// </summary>
        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        /// <summary>
        /// In message.
        /// </summary>
        [JsonPropertyName("in_msg")]
        public TonscanTransactionMsg? InMsg { get; set; }

        /// <summary>
        /// Out messages.
        /// </summary>
        [JsonPropertyName("out_msgs")]
        public IList<TonscanTransactionMsg> OutMsgs { get; set; } = new List<TonscanTransactionMsg>();
    }
}