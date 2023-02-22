// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorerAccountCrc20Transfers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer CRC-20 transfers.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class CscExplorerAccountCrc20Transfers :
        ICscExplorerTransferList<CscExplorerAccountCrc20TransferData, CscExplorerAccountCrc20TransferRecord>
    {
        /// <summary>
        /// Code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Transactions data.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public CscExplorerAccountCrc20TransferData? Data { get; set; }
    }
}