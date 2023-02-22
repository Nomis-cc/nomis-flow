// ------------------------------------------------------------------------------------------------------
// <copyright file="FlareExplorerSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.FlareExplorer.Settings
{
    /// <summary>
    /// Flare Explorer settings.
    /// </summary>
    internal class FlareExplorerSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://songbird-explorer.flare.network/api-docs"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}