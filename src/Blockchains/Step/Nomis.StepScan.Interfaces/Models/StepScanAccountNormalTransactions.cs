// ------------------------------------------------------------------------------------------------------
// <copyright file="StepScanAccountNormalTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.StepScan.Interfaces.Models
{
    /// <summary>
    /// StepScan account normal transactions.
    /// </summary>
    public class StepScanAccountNormalTransactions :
        IStepScanTransferList<StepScanAccountNormalTransaction>
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
        /// Account normal transaction list.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<StepScanAccountNormalTransaction> Data { get; set; } = new List<StepScanAccountNormalTransaction>();
    }
}