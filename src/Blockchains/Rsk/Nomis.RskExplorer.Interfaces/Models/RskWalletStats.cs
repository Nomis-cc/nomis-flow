// ------------------------------------------------------------------------------------------------------
// <copyright file="RskWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.RskExplorer.Interfaces.Models
{
    /// <summary>
    /// RSK wallet stats.
    /// </summary>
    public sealed class RskWalletStats :
        BaseEvmWalletStats<RskTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "RBTC";
    }
}