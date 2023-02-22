﻿// ------------------------------------------------------------------------------------------------------
// <copyright file="SnowtraceAccountERC20TokenEvents.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Snowtrace.Interfaces.Models
{
    /// <summary>
    /// Snowtrace account ERC-20 token transfer events.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class SnowtraceAccountERC20TokenEvents :
        ISnowtraceTransferList<SnowtraceAccountERC20TokenEvent>
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
        /// Account ERC-20 token event list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<SnowtraceAccountERC20TokenEvent> Data { get; set; } = new List<SnowtraceAccountERC20TokenEvent>();
    }
}