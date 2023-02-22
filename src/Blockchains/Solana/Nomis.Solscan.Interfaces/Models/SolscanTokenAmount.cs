// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanTokenAmount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text.Json.Serialization;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan token amount.
    /// </summary>
    public class SolscanTokenAmount
    {
        /// <summary>
        /// Amount for UI.
        /// </summary>
        [JsonPropertyName("uiAmount")]
        public double UiAmount { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

        /// <summary>
        /// Decimals.
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return UiAmount.ToString(CultureInfo.InvariantCulture);
        }
    }
}