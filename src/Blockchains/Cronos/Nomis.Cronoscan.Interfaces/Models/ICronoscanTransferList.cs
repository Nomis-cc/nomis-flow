// ------------------------------------------------------------------------------------------------------
// <copyright file="ICronoscanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Cronoscan.Interfaces.Models
{
    /// <summary>
    /// Cronoscan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Cronoscan transfer.</typeparam>
    public interface ICronoscanTransferList<TListItem>
        where TListItem : ICronoscanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}