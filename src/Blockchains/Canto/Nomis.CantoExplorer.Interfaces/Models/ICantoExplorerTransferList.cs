// ------------------------------------------------------------------------------------------------------
// <copyright file="ICantoExplorerTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.CantoExplorer.Interfaces.Models
{
    /// <summary>
    /// Canto Explorer transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Canto Explorer transfer.</typeparam>
    public interface ICantoExplorerTransferList<TListItem>
        where TListItem : ICantoExplorerTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}