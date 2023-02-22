// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeAccount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node account data.
    /// </summary>
    public class HederaMirrorNodeAccount
    {
        /// <summary>
        /// The ID of the account.
        /// </summary>
        [JsonPropertyName("account")]
        public string? Account { get; set; }

        /// <summary>
        /// RFC4648 no-padding base32 encoded account alias.
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }

        /// <summary>
        /// The timestamp and account balance of the account.
        /// </summary>
        [JsonPropertyName("balance")]
        public HederaMirrorNodeAccountBalance? Balance { get; set; }

        /// <summary>
        /// The timestamp of when the account was created.
        /// </summary>
        [JsonPropertyName("created_timestamp")]
        public string? CreatedTimestamp { get; set; }

        /// <summary>
        /// Whether the account was deleted or not.
        /// </summary>
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// A network entity encoded as an EVM encoded hex.
        /// </summary>
        [JsonPropertyName("evm_address")]
        public string? EvmAddress { get; set; }

        /// <summary>
        /// The public key associated with the account.
        /// </summary>
        [JsonPropertyName("key")]
        public HederaMirrorNodeAccountKey? Key { get; set; }

        /// <summary>
        /// The number of automatic token associations, if any.
        /// </summary>
        [JsonPropertyName("max_automatic_token_associations")]
        public int MaxAutomaticTokenAssociations { get; set; }

        /// <summary>
        /// The account memo, if any.
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }

        /// <summary>
        /// The account transactions.
        /// </summary>
        [JsonPropertyName("transactions")]
        public IList<HederaMirrorNodeAccountTransaction> Transactions { get; set; } = new List<HederaMirrorNodeAccountTransaction>();

        /// <summary>
        /// Hyperlinks.
        /// </summary>
        [JsonPropertyName("links")]
        public HederaMirrorNodeAccountLinks? Links { get; set; }
    }
}