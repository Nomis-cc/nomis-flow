// ------------------------------------------------------------------------------------------------------
// <copyright file="SnowtraceAccountInternalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Snowtrace.Interfaces.Models
{
    /// <summary>
    /// Snowtrace account internal transactions.
    /// </summary>
    public class SnowtraceAccountInternalTransactions :
        ISnowtraceTransferList<SnowtraceAccountInternalTransaction>
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
        public IList<SnowtraceAccountInternalTransaction> Data { get; set; } = new List<SnowtraceAccountInternalTransaction>();
    }
}