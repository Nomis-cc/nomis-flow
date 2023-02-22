// ------------------------------------------------------------------------------------------------------
// <copyright file="IKlaytnscopeTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope transfer list.
    /// </summary>
    /// <typeparam name="TListItem">Klaytnscope transfer.</typeparam>
    public interface IKlaytnscopeTransferList<TListItem>
        where TListItem : class
    {
        /// <summary>
        /// Account normal transaction list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem>? Result { get; set; }

        /// <summary>
        /// Page.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// Limit.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Total.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}