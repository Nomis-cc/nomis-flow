// ------------------------------------------------------------------------------------------------------
// <copyright file="IBttcscanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Bttcscan.Interfaces.Models
{
    /// <summary>
    /// Bttcscan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Bttcscan transfer.</typeparam>
    public interface IBttcscanTransferList<TListItem>
        where TListItem : IBttcscanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}