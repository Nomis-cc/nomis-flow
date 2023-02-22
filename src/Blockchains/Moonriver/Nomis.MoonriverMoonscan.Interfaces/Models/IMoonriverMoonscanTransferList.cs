// ------------------------------------------------------------------------------------------------------
// <copyright file="IMoonriverMoonscanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.MoonriverMoonscan.Interfaces.Models
{
    /// <summary>
    /// Moonriver moonscan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Moonriver moonscan transfer.</typeparam>
    public interface IMoonriverMoonscanTransferList<TListItem>
        where TListItem : IMoonriverMoonscanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}