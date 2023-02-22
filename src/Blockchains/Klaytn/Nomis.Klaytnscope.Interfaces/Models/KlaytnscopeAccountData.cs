// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountData.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account data.
    /// </summary>
    public class KlaytnscopeAccountData
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Address name.
        /// </summary>
        [JsonPropertyName("addressName")]
        public string? AddressName { get; set; }

        /// <summary>
        /// Balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public string? Balance { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }

        /// <summary>
        /// Transfers count.
        /// </summary>
        [JsonPropertyName("transferCount")]
        public int TransferCount { get; set; }

        /// <summary>
        /// NFT transfers count.
        /// </summary>
        [JsonPropertyName("nftTransferCount")]
        public int NftTransferCount { get; set; }

        /// <summary>
        /// Transactions count.
        /// </summary>
        [JsonPropertyName("txCount")]
        public int TxCount { get; set; }

        /// <summary>
        /// Events count.
        /// </summary>
        [JsonPropertyName("eventCount")]
        public int EventCount { get; set; }

        /// <summary>
        /// Internal transactions count.
        /// </summary>
        [JsonPropertyName("internalTxCount")]
        public int InternalTxCount { get; set; }

        /// <summary>
        /// Creator.
        /// </summary>
        [JsonPropertyName("creator")]
        public string? Creator { get; set; }

        /// <summary>
        /// Creation transaction hash.
        /// </summary>
        [JsonPropertyName("creationTxHash")]
        public string? CreationTxHash { get; set; }

        /// <summary>
        /// Delegates count.
        /// </summary>
        [JsonPropertyName("delegateCount")]
        public int DelegateCount { get; set; }

        /// <summary>
        /// Creator name.
        /// </summary>
        [JsonPropertyName("creatorName")]
        public string? CreatorName { get; set; }
    }
}