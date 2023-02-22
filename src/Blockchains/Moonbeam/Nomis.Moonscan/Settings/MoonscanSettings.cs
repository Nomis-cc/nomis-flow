// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonscanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.Moonscan.Settings
{
    /// <summary>
    /// Moonscan settings.
    /// </summary>
    internal class MoonscanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for moonscan.
        /// </summary>
        /// <remarks>
        /// <see href="https://moonbeam.moonscan.io/apis"/>
        /// </remarks>
        public string? ApiKey { get; set; }

        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://moonbeam.moonscan.io/apis#accounts"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}