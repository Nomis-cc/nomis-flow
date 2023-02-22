// ------------------------------------------------------------------------------------------------------
// <copyright file="IAurorascanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Aurorascan.Interfaces.Models
{
    /// <summary>
    /// Aurorascan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Aurorascan transfer.</typeparam>
    public interface IAurorascanTransferList<TListItem>
        where TListItem : IAurorascanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}