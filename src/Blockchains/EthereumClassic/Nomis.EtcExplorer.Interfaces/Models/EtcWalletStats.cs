// ------------------------------------------------------------------------------------------------------
// <copyright file="EtcWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.EtcExplorer.Interfaces.Models
{
    /// <summary>
    /// Etc wallet stats.
    /// </summary>
    public sealed class EtcWalletStats :
        BaseEvmWalletStats<EtcTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "ETC";
    }
}