// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanOrder.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan order.
    /// </summary>
    public class XrpscanOrder
    {
        /// <summary>
        /// Specification.
        /// </summary>
        [JsonPropertyName("specification")]
        public XrpscanOrderSpecification? Specification { get; set; }

        /// <summary>
        /// Properties.
        /// </summary>
        [JsonPropertyName("properties")]
        public XrpscanOrderProperties? Properties { get; set; }
    }
}