// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account transfer data.
    /// </summary>
    public class TronscanAccountTransfer :
        ITronscanTransfer
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("block")]
        public long Block { get; set; }

        /// <summary>
        /// Transaction hash.
        /// </summary>
        [JsonPropertyName("transactionHash")]
        public string? TransactionHash { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Transfer from address.
        /// </summary>
        [JsonPropertyName("transferFromAddress")]
        public string? TransferFromAddress { get; set; }

        /// <summary>
        /// Transfer to address.
        /// </summary>
        [JsonPropertyName("transferToAddress")]
        public string? TransferToAddress { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Token name.
        /// </summary>
        [JsonPropertyName("tokenName")]
        public string? TokenName { get; set; }

        /// <summary>
        /// Confirmed.
        /// </summary>
        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        /// <summary>
        /// Data.
        /// </summary>
        [JsonPropertyName("data")]
        public string? Data { get; set; }

        /// <summary>
        /// Contract return.
        /// </summary>
        [JsonPropertyName("contractRet")]
        public string? ContractRet { get; set; }

        /// <summary>
        /// Revert.
        /// </summary>
        [JsonPropertyName("revert")]
        public bool Revert { get; set; }

        /// <summary>
        /// Token info.
        /// </summary>
        [JsonPropertyName("tokenInfo")]
        public TronscanAccountTokenInfo? TokenInfo { get; set; }
    }
}