// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonriverMoonscanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.MoonriverMoonscan.Settings
{
    /// <summary>
    /// Moonriver moonscan settings.
    /// </summary>
    internal class MoonriverMoonscanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for moonriver moonscan.
        /// </summary>
        /// <remarks>
        /// <see href="https://moonriver.moonscan.io/apis"/>
        /// </remarks>
        public string? ApiKey { get; set; }

        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://moonriver.moonscan.io/apis#accounts"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}