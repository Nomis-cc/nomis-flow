// ------------------------------------------------------------------------------------------------------
// <copyright file="ITronscanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Tronscan.Interfaces.Models
{
    /// <summary>
    /// Tronscan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Tronscan transfer.</typeparam>
    public interface ITronscanTransferList<TListItem>
        where TListItem : ITronscanTransfer
    {
        /// <summary>
        /// Total transfer count.
        /// </summary>
        [JsonPropertyName("rangeTotal")]
        public long RangeTotal { get; set; }

        /// <summary>
        /// Total transfer count for this page.
        /// </summary>
        [JsonPropertyName("total")]
        public long Total { get; set; }

        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem>? Data { get; set; }
    }
}