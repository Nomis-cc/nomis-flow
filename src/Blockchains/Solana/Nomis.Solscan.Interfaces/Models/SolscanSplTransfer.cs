// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanSplTransfer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Utils.Extensions;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan Spl transfer.
    /// </summary>
    public class SolscanSplTransfer
        : ISolscanTransfer
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Change type.
        /// </summary>
        [JsonPropertyName("changeType")]
        public string? ChangeType { get; set; }

        /// <summary>
        /// Change amount.
        /// </summary>
        [JsonPropertyName("changeAmount")]
        public long ChangeAmount { get; set; }

        /// <summary>
        /// Decimals.
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        /// <summary>
        /// Post balance.
        /// </summary>
        [JsonPropertyName("postBalance")]
        public object? PostBalance { get; set; }

        /// <summary>
        /// Pre balance.
        /// </summary>
        [JsonPropertyName("preBalance")]
        public long PreBalance { get; set; }

        /// <summary>
        /// Token address.
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

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
        /// Spl transfer balance.
        /// </summary>
        [JsonPropertyName("balance")]
        public SolscanSplTransferBalance? Balance { get; set; }
    }
}