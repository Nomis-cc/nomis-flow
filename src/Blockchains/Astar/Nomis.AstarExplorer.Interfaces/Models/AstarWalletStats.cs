// ------------------------------------------------------------------------------------------------------
// <copyright file="AstarWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.AstarExplorer.Interfaces.Models
{
    /// <summary>
    /// Astar wallet stats.
    /// </summary>
    public sealed class AstarWalletStats :
        BaseEvmWalletStats<AstarTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "ASTR";
    }
}