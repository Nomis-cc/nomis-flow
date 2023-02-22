﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="NovaArbiscanAccountERC721TokenEvents.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.NovaArbiscan.Interfaces.Models
{
    /// <summary>
    /// Nova arbiscan account ERC-721 token transfer events.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class NovaArbiscanAccountERC721TokenEvents :
        INovaArbiscanTransferList<NovaArbiscanAccountERC721TokenEvent>
    {
        /// <summary>
        /// Status.
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Account ERC-721 token event list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<NovaArbiscanAccountERC721TokenEvent> Data { get; set; } = new List<NovaArbiscanAccountERC721TokenEvent>();
    }
}