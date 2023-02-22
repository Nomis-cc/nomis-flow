// ------------------------------------------------------------------------------------------------------
// <copyright file="IAeternityExplorerTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.AeternityExplorer.Interfaces.Models
{
    /// <summary>
    /// AeternityExplorer transfer list.
    /// </summary>
    /// <typeparam name="TListItem">AeternityExplorer transfer.</typeparam>
    public interface IAeternityExplorerTransferList<TListItem>
        where TListItem : class
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem>? Data { get; set; }

        /// <summary>
        /// Next list of transfers.
        /// </summary>
        [JsonPropertyName("next")]
        public string? Next { get; set; }

        /// <summary>
        /// Previous list of transfers.
        /// </summary>
        [JsonPropertyName("prev")]
        public string? Prev { get; set; }
    }
}