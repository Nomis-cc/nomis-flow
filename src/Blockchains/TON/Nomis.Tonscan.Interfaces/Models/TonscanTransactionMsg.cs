// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanTransactionMsg.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Interfaces.Models
{
    /// <summary>
    /// Tonscan transaction message data.
    /// </summary>
    public class TonscanTransactionMsg
    {
        /// <summary>
        /// Source.
        /// </summary>
        [JsonPropertyName("source")]
        public TonscanAccount? Source { get; set; }

        /// <summary>
        /// Destination.
        /// </summary>
        [JsonPropertyName("destination")]
        public TonscanAccount? Destination { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [JsonPropertyName("value")]
        public BigInteger Value { get; set; }

        /// <summary>
        /// Created last transaction.
        /// </summary>
        [JsonPropertyName("created_lt")]
        public BigInteger CreatedLt { get; set; }
    }
}