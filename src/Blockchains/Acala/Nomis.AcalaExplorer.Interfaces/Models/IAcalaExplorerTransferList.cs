// ------------------------------------------------------------------------------------------------------
// <copyright file="IAcalaExplorerTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.AcalaExplorer.Interfaces.Models
{
    /// <summary>
    /// Acala Explorer transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Acala Explorer transfer.</typeparam>
    public interface IAcalaExplorerTransferList<TListItem>
        where TListItem : IAcalaExplorerTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}