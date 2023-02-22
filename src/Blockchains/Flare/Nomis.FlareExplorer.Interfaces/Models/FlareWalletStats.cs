// ------------------------------------------------------------------------------------------------------
// <copyright file="FlareWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.FlareExplorer.Interfaces.Models
{
    /// <summary>
    /// Flare wallet stats.
    /// </summary>
    public sealed class FlareWalletStats :
        BaseEvmWalletStats<FlareTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "SGB";
    }
}