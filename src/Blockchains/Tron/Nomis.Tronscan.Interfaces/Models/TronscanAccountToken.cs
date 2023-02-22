// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountToken.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account token data.
    /// </summary>
    public class TronscanAccountToken :
        ITronscanAccountBalanceData
    {
        /// <summary>
        /// The token amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The token price in TRX.
        /// </summary>
        [JsonPropertyName("tokenPriceInTrx")]
        public decimal TokenPriceInTrx { get; set; }

        /// <summary>
        /// The token id.
        /// </summary>
        [JsonPropertyName("tokenId")]
        public string? TokenId { get; set; }

        /// <summary>
        /// The token balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public string? Balance { get; set; }

        /// <summary>
        /// The token name.
        /// </summary>
        [JsonPropertyName("tokenName")]
        public string? TokenName { get; set; }

        /// <summary>
        /// The token decimal.
        /// </summary>
        [JsonPropertyName("tokenDecimal")]
        public int TokenDecimal { get; set; }

        /// <summary>
        /// The token abbr.
        /// </summary>
        [JsonPropertyName("tokenAbbr")]
        public string? TokenAbbr { get; set; }

        /// <summary>
        /// The token can show.
        /// </summary>
        [JsonPropertyName("tokenCanShow")]
        public int TokenCanShow { get; set; }

        /// <summary>
        /// The token type.
        /// </summary>
        [JsonPropertyName("tokenType")]
        public string? TokenType { get; set; }

        /// <summary>
        /// VIP.
        /// </summary>
        [JsonPropertyName("vip")]
        public bool Vip { get; set; }

        /// <summary>
        /// The token logo.
        /// </summary>
        [JsonPropertyName("tokenLogo")]
        public string? TokenLogo { get; set; }
    }
}