// ------------------------------------------------------------------------------------------------------
// <copyright file="CubescanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.Cubescan.Settings
{
    /// <summary>
    /// Cubescan settings.
    /// </summary>
    internal class CubescanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for cubescan.
        /// </summary>
        /// <remarks>
        /// <see href="https://www.cubescan.network/en-us/apis"/>
        /// </remarks>
        public string? ApiKey { get; set; }

        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://www.cubescan.network/en-us/apis"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}