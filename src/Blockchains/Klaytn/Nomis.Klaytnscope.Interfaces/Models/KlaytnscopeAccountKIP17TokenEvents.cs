// ------------------------------------------------------------------------------------------------------
// <copyright file="KlaytnscopeAccountKIP17TokenEvents.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account KIP-17 token transfer events.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class KlaytnscopeAccountKIP17TokenEvents :
        IKlaytnscopeTransferList<KlaytnscopeAccountKIP17TokenEvent>
    {
        /// <summary>
        /// Success.
        /// </summary>
        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        /// <summary>
        /// Code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Account KIP-17 token event list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<KlaytnscopeAccountKIP17TokenEvent>? Result { get; set; } = new List<KlaytnscopeAccountKIP17TokenEvent>();

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

        /// <summary>
        /// Account KIP-17 token dictionary.
        /// </summary>
        [JsonPropertyName("tokens")]
        [DataMember(EmitDefaultValue = true)]
        public IDictionary<string, KlaytnscopeAccountKIP17TokenData>? Tokens { get; set; } = new Dictionary<string, KlaytnscopeAccountKIP17TokenData>();
    }
}