// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountInternalTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account internal transaction.
    /// </summary>
    public class KlaytnscopeAccountInternalTransaction :
        IKlaytnscopeTransfer
    {
        /// <summary>
        /// Created at.
        /// </summary>
        [JsonPropertyName("createdAt")]
        public string? CreatedAt { get; set; }

        /// <summary>
        /// Parent hash.
        /// </summary>
        [JsonPropertyName("parentHash")]
        public string? ParentHash { get; set; }

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

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Method name.
        /// </summary>
        [JsonPropertyName("methodName")]
        public string? MethodName { get; set; }
    }
}