// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountNormalTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account normal transaction data.
    /// </summary>
    public class TronscanAccountNormalTransaction :
        ITronscanTransfer
    {
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
        /// Timestamp.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Owner address.
        /// </summary>
        [JsonPropertyName("ownerAddress")]
        public string? OwnerAddress { get; set; }

        /// <summary>
        /// To address list.
        /// </summary>
        [JsonPropertyName("toAddressList")]
        public IList<string>? ToAddressList { get; set; } = new List<string>();

        /// <summary>
        /// To address.
        /// </summary>
        [JsonPropertyName("toAddress")]
        public string? ToAddress { get; set; }

        /// <summary>
        /// Contract type.
        /// </summary>
        [JsonPropertyName("contractType")]
        public int ContractType { get; set; }

        /// <summary>
        /// Confirmed.
        /// </summary>
        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        /// <summary>
        /// Revert.
        /// </summary>
        [JsonPropertyName("revert")]
        public bool Revert { get; set; }

        /// <summary>
        /// Contract return.
        /// </summary>
        [JsonPropertyName("contractRet")]
        public string? ContractRet { get; set; }

        /// <summary>
        /// Result.
        /// </summary>
        [JsonPropertyName("result")]
        public string? Result { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Token info.
        /// </summary>
        [JsonPropertyName("tokenInfo")]
        public TronscanAccountTokenInfo? TokenInfo { get; set; }

        /// <summary>
        /// Token type.
        /// </summary>
        [JsonPropertyName("tokenType")]
        public string? TokenType { get; set; }
    }
}