// ------------------------------------------------------------------------------------------------------
// <copyright file="ISnowtraceTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Snowtrace.Interfaces.Models
{
    /// <summary>
    /// Snowtrace transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Snowtrace transfer.</typeparam>
    public interface ISnowtraceTransferList<TListItem>
        where TListItem : ISnowtraceTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}