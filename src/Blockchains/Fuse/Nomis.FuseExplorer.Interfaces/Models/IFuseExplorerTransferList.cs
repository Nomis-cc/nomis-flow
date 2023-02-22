// ------------------------------------------------------------------------------------------------------
// <copyright file="IFuseExplorerTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.FuseExplorer.Interfaces.Models
{
    /// <summary>
    /// Fuse Explorer transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Fuse Explorer transfer.</typeparam>
    public interface IFuseExplorerTransferList<TListItem>
        where TListItem : IFuseExplorerTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}