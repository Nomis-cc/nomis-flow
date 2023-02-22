// ------------------------------------------------------------------------------------------------------
// <copyright file="FuseWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.FuseExplorer.Interfaces.Models
{
    /// <summary>
    /// Fuse wallet stats.
    /// </summary>
    public sealed class FuseWalletStats :
        BaseEvmWalletStats<FuseTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "FUSE";
    }
}