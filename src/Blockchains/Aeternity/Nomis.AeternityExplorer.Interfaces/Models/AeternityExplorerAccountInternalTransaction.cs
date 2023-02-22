// ------------------------------------------------------------------------------------------------------
// <copyright file="AeternityExplorerAccountInternalTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer account internal transaction.
    /// </summary>
    public class AeternityExplorerAccountInternalTransaction
    {
        /// <summary>
        /// Account identifier.
        /// </summary>
        [JsonPropertyName("account_id")]
        public string? AccountId { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public BigInteger Amount { get; set; }

        /// <summary>
        /// Block height.
        /// </summary>
        [JsonPropertyName("height")]
        public long Height { get; set; }

        /// <summary>
        /// Kind.
        /// </summary>
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        /// <summary>
        /// Reference transaction.
        /// </summary>
        [JsonPropertyName("ref_txi")]
        public long? RefTxi { get; set; }
    }
}