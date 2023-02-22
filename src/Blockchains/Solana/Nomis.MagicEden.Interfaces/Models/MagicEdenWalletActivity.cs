// ------------------------------------------------------------------------------------------------------
// <copyright file="MagicEdenWalletActivity.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Utils.Extensions;

namespace Nomis.MagicEden.Interfaces.Models
{
    /// <summary>
    /// MagicEden wallet activity.
    /// </summary>
    public class MagicEdenWalletActivity
    {
        /// <summary>
        /// Signature.
        /// </summary>
        [JsonPropertyName("signature")]
        public string? Signature { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Source.
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        /// <summary>
        /// Token mint.
        /// </summary>
        [JsonPropertyName("tokenMint")]
        public string? TokenMint { get; set; }

        /// <summary>
        /// Collection.
        /// </summary>
        [JsonPropertyName("collection")]
        public string? Collection { get; set; }

        /// <summary>
        /// Collection symbol.
        /// </summary>
        [JsonPropertyName("collectionSymbol")]
        public string? CollectionSymbol { get; set; }

        /// <summary>
        /// Slot.
        /// </summary>
        [JsonPropertyName("slot")]
        public long Slot { get; set; }

        /// <summary>
        /// The block time.
        /// </summary>
        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        /// <summary>
        /// The block time.
        /// </summary>
        public DateTime Date => BlockTime.ToString().ToDateTime();

        /// <summary>
        /// Buyer.
        /// </summary>
        [JsonPropertyName("buyer")]
        public string? Buyer { get; set; }

        /// <summary>
        /// Buyer referral.
        /// </summary>
        [JsonPropertyName("buyerReferral")]
        public string? BuyerReferral { get; set; }

        /// <summary>
        /// Seller.
        /// </summary>
        [JsonPropertyName("seller")]
        public string? Seller { get; set; }

        /// <summary>
        /// Seller referral.
        /// </summary>
        [JsonPropertyName("sellerReferral")]
        public string? SellerReferral { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}