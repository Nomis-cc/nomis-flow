// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeNftTransactionData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node NFT transaction data.
    /// </summary>
    public class HederaMirrorNodeNftTransactionData
    {
        /// <summary>
        /// The consensus timestamp in seconds.nanoseconds.
        /// </summary>
        [JsonPropertyName("consensus_timestamp")]
        public string? ConsensusTimestamp { get; set; }

        /// <summary>
        /// The ID of the NFT receiver.
        /// </summary>
        [JsonPropertyName("receiver_account_id")]
        public string? ReceiverAccountId { get; set; }

        /// <summary>
        /// The ID of the NFT sender.
        /// </summary>
        [JsonPropertyName("sender_account_id")]
        public string? SenderAccountId { get; set; }

        /// <summary>
        /// The ID of the transaction.
        /// </summary>
        [JsonPropertyName("transaction_id")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// The transaction type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}