// ------------------------------------------------------------------------------------------------------
// <copyright file="CronoscanAccountCRC721TokenEvents.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.Cronoscan.Interfaces.Models
{
    /// <summary>
    /// Cronoscan account CRC-721 token transfer events.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class CronoscanAccountCRC721TokenEvents :
        ICronoscanTransferList<CronoscanAccountCRC721TokenEvent>
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
        /// Account CRC-721 token event list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<CronoscanAccountCRC721TokenEvent> Data { get; set; } = new List<CronoscanAccountCRC721TokenEvent>();
    }
}