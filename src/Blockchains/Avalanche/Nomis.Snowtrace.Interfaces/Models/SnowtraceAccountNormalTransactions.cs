// ------------------------------------------------------------------------------------------------------
// <copyright file="SnowtraceAccountNormalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Snowtrace.Interfaces.Models
{
    /// <summary>
    /// Snowtrace account normal transactions.
    /// </summary>
    public class SnowtraceAccountNormalTransactions :
        ISnowtraceTransferList<SnowtraceAccountNormalTransaction>
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
        public IList<SnowtraceAccountNormalTransaction> Data { get; set; } = new List<SnowtraceAccountNormalTransaction>();
    }
}