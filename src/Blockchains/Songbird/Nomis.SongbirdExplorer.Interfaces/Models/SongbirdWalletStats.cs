// ------------------------------------------------------------------------------------------------------
// <copyright file="SongbirdWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Stats;

namespace Nomis.SongbirdExplorer.Interfaces.Models
{
    /// <summary>
    /// Songbird wallet stats.
    /// </summary>
    public sealed class SongbirdWalletStats :
        BaseEvmWalletStats<SongbirdTransactionIntervalData>
    {
        /// <inheritdoc/>
        public override string NativeToken => "SGB";
    }
}