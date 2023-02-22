// ------------------------------------------------------------------------------------------------------
// <copyright file="KavaWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.KavaExplorer.Interfaces.Models
{
    /// <summary>
    /// Kava wallet stats.
    /// </summary>
    public sealed class KavaWalletStats :
        BaseEvmWalletStats<KavaTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "KAVA";
    }
}