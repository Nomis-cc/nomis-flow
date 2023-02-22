// ------------------------------------------------------------------------------------------------------
// <copyright file="TronscanAccountContract.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan account contract data.
    /// </summary>
    public class TronscanAccountContract
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Verify status.
        /// </summary>
        [JsonPropertyName("verify_status")]
        public int VerifyStatus { get; set; }

        /// <summary>
        /// Balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Trx count.
        /// </summary>
        [JsonPropertyName("trxCount")]
        public int TrxCount { get; set; }

        /// <summary>
        /// Date created.
        /// </summary>
        [JsonPropertyName("date_created")]
        public long DateCreated { get; set; }

        /// <summary>
        /// VIP.
        /// </summary>
        [JsonPropertyName("vip")]
        public bool Vip { get; set; }
    }
}