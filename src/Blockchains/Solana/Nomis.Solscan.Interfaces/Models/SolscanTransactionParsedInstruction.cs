// ------------------------------------------------------------------------------------------------------
// <copyright file="SolscanTransactionParsedInstruction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Solscan.Interfaces.Models
{
    /// <summary>
    /// Solscan transaction parsed instruction.
    /// </summary>
    public class SolscanTransactionParsedInstruction
    {
        /// <summary>
        /// Program identifier.
        /// </summary>
        [JsonPropertyName("programId")]
        public string? ProgramId { get; set; }

        /// <summary>
        /// Program.
        /// </summary>
        [JsonPropertyName("program")]
        public string? Program { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}