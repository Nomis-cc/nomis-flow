// ------------------------------------------------------------------------------------------------------
// <copyright file="PoaWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.PoaExplorer.Interfaces.Models
{
    /// <summary>
    /// POA wallet stats.
    /// </summary>
    public sealed class PoaWalletStats :
        BaseEvmWalletStats<PoaTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "POA";
    }
}