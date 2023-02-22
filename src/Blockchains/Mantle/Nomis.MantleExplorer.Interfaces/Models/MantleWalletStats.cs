// ------------------------------------------------------------------------------------------------------
// <copyright file="MantleWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.MantleExplorer.Interfaces.Models
{
    /// <summary>
    /// Mantle wallet stats.
    /// </summary>
    public sealed class MantleWalletStats :
        BaseEvmWalletStats<MantleTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "BIT";
    }
}