// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeAccountBalance.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node account balance data.
    /// </summary>
    public class HederaMirrorNodeAccountBalance
    {
        /// <summary>
        /// The balance of the account.
        /// </summary>
        [JsonPropertyName("balance")]
        public BigInteger Balance { get; set; }

        /// <summary>
        /// The seconds.nanoseconds of the timestamp at which the balance for the account is returned.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        /// <summary>
        /// The tokens that are associated to this account.
        /// </summary>
        [JsonPropertyName("tokens")]
        public IList<HederaMirrorNodeAccountBalanceToken> Tokens { get; set; } = new List<HederaMirrorNodeAccountBalanceToken>();
    }
}