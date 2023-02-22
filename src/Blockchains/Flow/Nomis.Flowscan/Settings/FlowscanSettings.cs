// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowscanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.Flowscan.Settings
{
    /// <summary>
    /// Flowscan settings.
    /// </summary>
    internal class FlowscanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// Flowscan API key.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Flowscan GraphQL API base address.
        /// </summary>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}