// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Tonscan.Interfaces.Models
{
    /// <summary>
    /// Tonscan transactions data.
    /// </summary>
    public class TonscanTransactions
    {
        /// <summary>
        /// Transactions data.
        /// </summary>
        [JsonPropertyName("transactions")]
        public IList<TonscanTransaction> Transactions { get; set; } = new List<TonscanTransaction>();
    }
}