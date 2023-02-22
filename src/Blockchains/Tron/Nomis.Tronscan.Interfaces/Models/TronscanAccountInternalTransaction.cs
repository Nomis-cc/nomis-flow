// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountInternalTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account internal transaction data.
    /// </summary>
    public class TronscanAccountInternalTransaction :
        ITronscanTransfer
    {
        /// <summary>
        /// From address.
        /// </summary>
        [JsonPropertyName("from")]
        public string? From { get; set; }

        /// <summary>
        /// To address.
        /// </summary>
        [JsonPropertyName("to")]
        public string? To { get; set; }

        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("block")]
        public long Block { get; set; }

        /// <summary>
        /// Hash.
        /// </summary>
        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        /// <summary>
        /// Internal hash.
        /// </summary>
        [JsonPropertyName("internal_hash")]
        public string? InternalHash { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Rejected.
        /// </summary>
        [JsonPropertyName("rejected")]
        public bool Rejected { get; set; }

        /// <summary>
        /// Confirmed.
        /// </summary>
        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        /// <summary>
        /// Result.
        /// </summary>
        [JsonPropertyName("result")]
        public string? Result { get; set; }

        /// <summary>
        /// Revert.
        /// </summary>
        [JsonPropertyName("revert")]
        public bool Revert { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        [JsonPropertyName("note")]
        public string? Note { get; set; }

        /// <summary>
        /// Token list.
        /// </summary>
        [JsonPropertyName("token_list")]
        public TronscanAccountTokenList? TokenList { get; set; }

        /// <summary>
        /// The token id.
        /// </summary>
        [JsonPropertyName("tokenId")]
        public string? TokenId { get; set; }

        /// <summary>
        /// Call value.
        /// </summary>
        [JsonPropertyName("call_value")]
        public long CallValue { get; set; }
    }
}