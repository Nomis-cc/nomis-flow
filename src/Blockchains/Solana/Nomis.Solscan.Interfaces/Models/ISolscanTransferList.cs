// ------------------------------------------------------------------------------------------------------
// <copyright file="ISolscanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Solscan transfer.</typeparam>
    public interface ISolscanTransferList<TListItem>
        where TListItem : ISolscanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("data")]
        public IList<TListItem> Data { get; set; }
    }
}