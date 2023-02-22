// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountNormalTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account normal transaction.
    /// </summary>
    public class KlaytnscopeAccountNormalTransaction :
        IKlaytnscopeTransfer
    {
        /// <summary>
        /// Created at.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public BigInteger CreatedAt { get; set; }

        /// <summary>
        /// Transaction hash.
        /// </summary>
        [JsonPropertyName("txHash")]
        public string? TxHash { get; set; }

        /// <summary>
        /// Transaction type.
        /// </summary>
        [JsonPropertyName("txType")]
        public string? TxType { get; set; }

        /// <summary>
        /// Transaction status.
        /// </summary>
        [JsonPropertyName("txStatus")]
        public int TxStatus { get; set; }

        /// <summary>
        /// Method name.
        /// </summary>
        [JsonPropertyName("methodName")]
        public string? MethodName { get; set; }

        /// <summary>
        /// Block number.
        /// </summary>
        [JsonPropertyName("blockNumber")]
        public int BlockNumber { get; set; }

        /// <summary>
        /// From address.
        /// </summary>
        [JsonPropertyName("fromAddress")]
        public string? FromAddress { get; set; }

        /// <summary>
        /// To address.
        /// </summary>
        [JsonPropertyName("toAddress")]
        public string? ToAddress { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }
    }
}