// ------------------------------------------------------------------------------------------------------
// <copyright file="XrpscanOrderSpecification.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.Xrpscan.Interfaces.Models
{
    /// <summary>
    /// Xrpscan order specification.
    /// </summary>
    public class XrpscanOrderSpecification
    {
        /// <summary>
        /// Direction.
        /// </summary>
        [JsonPropertyName("direction")]
        public string? Direction { get; set; }

        /// <summary>
        /// Quantity.
        /// </summary>
        [JsonPropertyName("quantity")]
        public XrpscanOrderSpecificationQuantity? Quantity { get; set; }

        /// <summary>
        /// Total price.
        /// </summary>
        [JsonPropertyName("totalPrice")]
        public XrpscanOrderSpecificationTotalPrice? TotalPrice { get; set; }
    }
}