// ------------------------------------------------------------------------------------------------------
// <copyright file="ICloudwalkExplorerTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.CloudwalkExplorer.Interfaces.Models
{
    /// <summary>
    /// Cloudwalk Explorer transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Cloudwalk Explorer transfer.</typeparam>
    public interface ICloudwalkExplorerTransferList<TListItem>
        where TListItem : ICloudwalkExplorerTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}