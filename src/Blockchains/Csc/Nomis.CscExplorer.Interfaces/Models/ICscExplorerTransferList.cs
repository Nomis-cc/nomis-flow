// ------------------------------------------------------------------------------------------------------
// <copyright file="ICscExplorerTransferList.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Nomis.CscExplorer.Interfaces.Models
{
    /// <summary>
    /// CSC Explorer transfers data.
    /// </summary>
    /// <typeparam name="TData">CSC Explorer transfer data.</typeparam>
    /// <typeparam name="TRecord">CSC Explorer transfer record.</typeparam>
    public interface ICscExplorerTransferList<TData, TRecord>
        where TData : ICscExplorerTransferData<TRecord>
        where TRecord : ICscExplorerTransferRecord
    {
        /// <summary>
        /// Transfers data.
        /// </summary>
        [JsonPropertyName("data")]
        [DataMember(EmitDefaultValue = true)]
        public TData? Data { get; set; }
    }
}