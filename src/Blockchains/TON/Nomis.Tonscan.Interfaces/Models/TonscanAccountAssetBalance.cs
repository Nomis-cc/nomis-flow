// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanAccountAssetBalance.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Interfaces.Models
{
    /// <summary>
    /// Tonscan account asset balance data.
    /// </summary>
    public class TonscanAccountAssetBalance
    {
        /// <summary>
        /// Balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public string? Balance { get; set; }

        /// <summary>
        /// Jetton address.
        /// </summary>
        [JsonPropertyName("jetton_address")]
        public string? JettonAddress { get; set; }

        /// <summary>
        /// Metadata.
        /// </summary>
        [JsonPropertyName("metadata")]
        public TonscanAccountAssetBalanceMetadata? Metadata { get; set; }

        /// <summary>
        /// Verification.
        /// </summary>
        [JsonPropertyName("verification")]
        public string? Verification { get; set; }

        /// <summary>
        /// Wallet address.
        /// </summary>
        [JsonPropertyName("wallet_address")]
        public TonscanAccount? WalletAddress { get; set; }
    }
}