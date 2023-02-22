// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeNftTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node NFT transactions data.
    /// </summary>
    public class HederaMirrorNodeNftTransactions
    {
        /// <summary>
        /// The account NFT transactions.
        /// </summary>
        [JsonPropertyName("transactions")]
        public IList<HederaMirrorNodeNftTransactionData> Transactions { get; set; } = new List<HederaMirrorNodeNftTransactionData>();

        /// <summary>
        /// Hyperlinks.
        /// </summary>
        [JsonPropertyName("links")]
        public HederaMirrorNodeAccountLinks? Links { get; set; }
    }
}