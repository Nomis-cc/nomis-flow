// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNodeNfts.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.HederaMirrorNode.Interfaces.Models
{
    /// <summary>
    /// Hedera Mirror Node NFTs data.
    /// </summary>
    public class HederaMirrorNodeNfts
    {
        /// <summary>
        /// NFTs by account ID.
        /// </summary>
        [JsonPropertyName("nfts")]
        public IList<HederaMirrorNodeNftData> Nfts { get; set; } = new List<HederaMirrorNodeNftData>();

        /// <summary>
        /// Hyperlinks.
        /// </summary>
        [JsonPropertyName("links")]
        public HederaMirrorNodeAccountLinks? Links { get; set; }
    }
}