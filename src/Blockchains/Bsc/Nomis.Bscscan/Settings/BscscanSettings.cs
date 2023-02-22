// ------------------------------------------------------------------------------------------------------
// <copyright file="BscscanSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.Bscscan.Settings
{
    /// <summary>
    /// Bscscan settings.
    /// </summary>
    internal class BscscanSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// API key for Bscscan.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Bscscan API base URL.
        /// </summary>
        public string? ApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}