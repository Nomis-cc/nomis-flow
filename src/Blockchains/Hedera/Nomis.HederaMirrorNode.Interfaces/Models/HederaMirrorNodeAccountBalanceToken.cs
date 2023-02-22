// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeAccountBalanceToken.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node account balance token data.
    /// </summary>
    public class HederaMirrorNodeAccountBalanceToken
    {
        /// <summary>
        /// The ID of the token associated to this account.
        /// </summary>
        [JsonPropertyName("token_id")]
        public string? TokenId { get; set; }

        /// <summary>
        /// The token balance for the specified token associated to this account.
        /// </summary>
        [JsonPropertyName("balance")]
        public BigInteger Balance { get; set; }
    }
}