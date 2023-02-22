// ------------------------------------------------------------------------------------------------------
// <copyright file="ICubescanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Cubescan.Interfaces.Models
{
    /// <summary>
    /// Cubescan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Cubescan transfer.</typeparam>
    public interface ICubescanTransferList<TListItem>
        where TListItem : ICubescanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}