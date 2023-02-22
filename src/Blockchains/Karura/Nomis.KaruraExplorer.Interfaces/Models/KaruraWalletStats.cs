// ------------------------------------------------------------------------------------------------------
// <copyright file="KaruraWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.KaruraExplorer.Interfaces.Models
{
    /// <summary>
    /// Karura wallet stats.
    /// </summary>
    public sealed class KaruraWalletStats :
        BaseEvmWalletStats<KaruraTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "KAR";
    }
}