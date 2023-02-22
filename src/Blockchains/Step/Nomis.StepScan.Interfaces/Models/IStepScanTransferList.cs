// ------------------------------------------------------------------------------------------------------
// <copyright file="IStepScanTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.StepScan.Interfaces.Models
{
    /// <summary>
    /// StepScan transfer list.
    /// </summary>
    /// <typeparam name="TListItem">StepScan transfer.</typeparam>
    public interface IStepScanTransferList<TListItem>
        where TListItem : IStepScanTransfer
    {
        /// <summary>
        /// List of transfers.
        /// </summary>
        [JsonPropertyName("result")]
        [DataMember(EmitDefaultValue = true)]
        public IList<TListItem> Data { get; set; }
    }
}