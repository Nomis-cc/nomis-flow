// ------------------------------------------------------------------------------------------------------
// <copyright file="BobascanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.Bobascan.Settings
{
    /// <summary>
    /// Bobascan settings.
    /// </summary>
    internal class BobascanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for bobascan.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://bobascan.com/apis"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}