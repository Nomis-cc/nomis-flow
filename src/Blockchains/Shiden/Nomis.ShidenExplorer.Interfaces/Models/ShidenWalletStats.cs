// ------------------------------------------------------------------------------------------------------
// <copyright file="ShidenWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.ShidenExplorer.Interfaces.Models
{
    /// <summary>
    /// Shiden wallet stats.
    /// </summary>
    public sealed class ShidenWalletStats :
        BaseEvmWalletStats<ShidenTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "SDN";
    }
}