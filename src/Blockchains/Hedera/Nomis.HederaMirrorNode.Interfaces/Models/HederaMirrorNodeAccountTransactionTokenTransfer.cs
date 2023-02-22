// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeAccountTransactionTokenTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node account transaction token transfer data.
    /// </summary>
    public class HederaMirrorNodeAccountTransactionTokenTransfer
    {
        /// <summary>
        /// The ID of the transferred token.
        /// </summary>
        [JsonPropertyName("token_id")]
        public string? TokenId { get; set; }

        /// <summary>
        /// The account for transfer.
        /// </summary>
        [JsonPropertyName("account")]
        public string? Account { get; set; }

        /// <summary>
        /// The amount of transferred tokens.
        /// </summary>
        /// <remarks>
        /// A negative (-) sign indicates a debit to that account.
        /// </remarks>
        [JsonPropertyName("amount")]
        public BigInteger Amount { get; set; }

        /// <summary>
        /// Is approval.
        /// </summary>
        [JsonPropertyName("is_approval")]
        public bool IsApproval { get; set; }
    }
}