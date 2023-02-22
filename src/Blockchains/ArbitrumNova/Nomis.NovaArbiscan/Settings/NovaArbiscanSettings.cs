// ------------------------------------------------------------------------------------------------------
// <copyright file="NovaArbiscanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.NovaArbiscan.Settings
{
    /// <summary>
    /// Nova arbiscan settings.
    /// </summary>
    internal class NovaArbiscanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for novaArbiscan.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://docs.arbiscan.io/getting-started/endpoint-urls"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}