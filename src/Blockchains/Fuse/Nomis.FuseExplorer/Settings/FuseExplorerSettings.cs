// ------------------------------------------------------------------------------------------------------
// <copyright file="FuseExplorerSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.FuseExplorer.Settings
{
    /// <summary>
    /// Fuse Explorer settings.
    /// </summary>
    internal class FuseExplorerSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for ftmscan.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://explorer.fuse.io/api-docs"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}