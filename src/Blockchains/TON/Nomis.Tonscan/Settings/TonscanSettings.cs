// ------------------------------------------------------------------------------------------------------
// <copyright file="TonscanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.Tonscan.Settings
{
    /// <summary>
    /// Ton Explorer settings.
    /// </summary>
    internal class TonscanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for tonscan.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://tonapi.io/docs"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <summary>
        /// GraphQL API base URL.
        /// </summary>
        public string? GraphQlApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}