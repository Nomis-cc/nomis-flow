// ------------------------------------------------------------------------------------------------------
// <copyright file="CantoWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.CantoExplorer.Interfaces.Models
{
    /// <summary>
    /// Canto wallet stats.
    /// </summary>
    public sealed class CantoWalletStats :
        BaseEvmWalletStats<CantoTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "CANTO";
    }
}