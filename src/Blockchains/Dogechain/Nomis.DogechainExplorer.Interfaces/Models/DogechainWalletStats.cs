// ------------------------------------------------------------------------------------------------------
// <copyright file="DogechainWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.DogechainExplorer.Interfaces.Models
{
    /// <summary>
    /// Dogechain wallet stats.
    /// </summary>
    public sealed class DogechainWalletStats :
        BaseEvmWalletStats<DogechainTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "Doge";
    }
}