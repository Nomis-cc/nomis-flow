// ------------------------------------------------------------------------------------------------------
// <copyright file="IBobascanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Bobascan.Interfaces.Models
{
    /// <summary>
    /// Bobascan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Bobascan transfer.</typeparam>
    public interface IBobascanTransferList<TListItem>
        where TListItem : IBobascanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem>? Data { get; set; }
    }
}