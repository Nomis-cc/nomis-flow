// ------------------------------------------------------------------------------------------------------
// <copyright file="VelasWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.VelasExplorer.Interfaces.Models
{
    /// <summary>
    /// Velas wallet stats.
    /// </summary>
    public sealed class VelasWalletStats :
        BaseEvmWalletStats<VelasTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "VLX";
    }
}