// ------------------------------------------------------------------------------------------------------
// <copyright file="IOasisExplorerTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.OasisExplorer.Interfaces.Models
{
    /// <summary>
    /// Oasis Explorer transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Oasis Explorer transfer.</typeparam>
    public interface IOasisExplorerTransferList<TListItem>
        where TListItem : IOasisExplorerTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}