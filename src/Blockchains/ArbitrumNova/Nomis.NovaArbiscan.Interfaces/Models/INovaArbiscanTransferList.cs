// ------------------------------------------------------------------------------------------------------
// <copyright file="INovaArbiscanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.NovaArbiscan.Interfaces.Models
{
    /// <summary>
    /// Nova arbiscan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Nova arbiscan transfer.</typeparam>
    public interface INovaArbiscanTransferList<TListItem>
        where TListItem : INovaArbiscanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}