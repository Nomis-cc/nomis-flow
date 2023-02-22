// ------------------------------------------------------------------------------------------------------
// <copyright file="AcalaWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.AcalaExplorer.Interfaces.Models
{
    /// <summary>
    /// Acala wallet stats.
    /// </summary>
    public sealed class AcalaWalletStats :
        BaseEvmWalletStats<AcalaTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "ACA";
    }
}