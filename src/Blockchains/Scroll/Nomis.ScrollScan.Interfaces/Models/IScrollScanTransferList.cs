// ------------------------------------------------------------------------------------------------------
// <copyright file="IScrollScanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.ScrollScan.Interfaces.Models
{
    /// <summary>
    /// ScrollScan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">ScrollScan transfer.</typeparam>
    public interface IScrollScanTransferList<TListItem>
        where TListItem : IScrollScanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}