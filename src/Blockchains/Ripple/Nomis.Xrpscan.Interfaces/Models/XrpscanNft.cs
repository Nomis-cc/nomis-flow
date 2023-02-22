// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanNft.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

// ReSharper disable InconsistentNaming

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan NFT data.
    /// </summary>
    public class XrpscanNft
    {
        /// <summary>
        /// Issuer.
        /// </summary>
        [JsonPropertyName("Issuer")]
        public string? Issuer { get; set; }

        /// <summary>
        /// NFT token identifier.
        /// </summary>
        [JsonPropertyName("NFTokenID")]
        public string? NftTokenId { get; set; }

        /// <summary>
        /// URI.
        /// </summary>
        [JsonPropertyName("URI")]
        public string? URI { get; set; }

        /// <summary>
        /// NFT serial.
        /// </summary>
        [JsonPropertyName("nft_serial")]
        public long NftSerial { get; set; }
    }
}