// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanKyc.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan KYC status.
    /// </summary>
    public class XrpscanKyc
    {
        /// <summary>
        /// Account.
        /// </summary>
        [JsonPropertyName("account")]
        public string? Account { get; set; }

        /// <summary>
        /// KYC approved.
        /// </summary>
        [JsonPropertyName("kycApproved")]
        public bool KycApproved { get; set; }
    }
}