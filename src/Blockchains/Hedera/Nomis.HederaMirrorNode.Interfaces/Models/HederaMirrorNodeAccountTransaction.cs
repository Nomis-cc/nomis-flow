// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeAccountTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node account transaction data.
    /// </summary>
    public class HederaMirrorNodeAccountTransaction
    {
        /// <summary>
        /// The consensus timestamp in seconds.nanoseconds.
        /// </summary>
        [JsonPropertyName("consensus_timestamp")]
        public string? ConsensusTimestamp { get; set; }

        /// <summary>
        /// The entity ID that is created from create transactions
        /// (AccountCreateTransaction, TopicCreateTransaction, TokenCreateTransaction,
        /// ScheduleCreateTransaction, ContractCreateTransaction, FileCreateTransaction).
        /// </summary>
        [JsonPropertyName("entity_id")]
        public string? EntityId { get; set; }

        /// <summary>
        /// The type of transaction.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The ID of the node that submitted the transaction to the network.
        /// </summary>
        [JsonPropertyName("node")]
        public string? Node { get; set; }

        /// <summary>
        /// Whether the cryptocurrency transaction was successful or not.
        /// </summary>
        [JsonPropertyName("result")]
        public string? Result { get; set; }

        /// <summary>
        /// The hash value of the transaction processed on the Hedera network.
        /// </summary>
        [JsonPropertyName("transaction_hash")]
        public string? TransactionHash { get; set; }

        /// <summary>
        /// The ID of the transaction.
        /// </summary>
        [JsonPropertyName("transaction_id")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// A list of the account IDs the crypto transfer occurred between and the amount that was transferred.
        /// </summary>
        /// <remarks>
        /// A negative (-) sign indicates a debit to that account. The transfer list includes the transfers
        /// between the from account and to account, the transfer of the node fee, the transfer of the network
        /// fee, and the transfer of the service fee for that transaction. If the transaction was not processed,
        /// a network fee is still assessed.
        /// </remarks>
        [JsonPropertyName("transfers")]
        public IList<HederaMirrorNodeAccountTransactionTransfer> Transfers { get; set; } = new List<HederaMirrorNodeAccountTransactionTransfer>();

        /// <summary>
        /// The token ID, account, and amount that was transferred to by this account in this transaction.
        /// </summary>
        /// <remarks>
        /// This will not be listed if it did not occur in the transaction.
        /// </remarks>
        [JsonPropertyName("token_transfers")]
        public IList<HederaMirrorNodeAccountTransactionTokenTransfer> TokenTransfers { get; set; } = new List<HederaMirrorNodeAccountTransactionTokenTransfer>();
    }
}