// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeNftData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node NFT data.
    /// </summary>
    public class HederaMirrorNodeNftData
    {
        /// <summary>
        /// The account ID of the account associated with the NFT.
        /// </summary>
        [JsonPropertyName("account_id")]
        public string? AccountId { get; set; }

        /// <summary>
        /// The timestamp of when the NFT was created.
        /// </summary>
        [JsonPropertyName("created_timestamp")]
        public string? CreatedTimestamp { get; set; }

        /// <summary>
        /// Whether the token was deleted or not.
        /// </summary>
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// The meta data of the NFT.
        /// </summary>
        [JsonPropertyName("metadata")]
        public string? Metadata { get; set; }

        /// <summary>
        /// The last time the token properties were modified.
        /// </summary>
        [JsonPropertyName("modified_timestamp")]
        public string? ModifiedTimestamp { get; set; }

        /// <summary>
        /// The serial number of the NFT.
        /// </summary>
        [JsonPropertyName("serial_number")]
        public long SerialNumber { get; set; }

        /// <summary>
        /// The token ID of the NFT.
        /// </summary>
        [JsonPropertyName("token_id")]
        public string? TokenId { get; set; }
    }
}