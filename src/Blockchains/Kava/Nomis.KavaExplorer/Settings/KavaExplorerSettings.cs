// ------------------------------------------------------------------------------------------------------
// <copyright file="KavaExplorerSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.KavaExplorer.Settings
{
    /// <summary>
    /// Kava Explorer settings.
    /// </summary>
    internal class KavaExplorerSettings :
        IBlockchainSettings
    {
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