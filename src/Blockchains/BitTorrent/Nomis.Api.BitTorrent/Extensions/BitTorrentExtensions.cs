// ------------------------------------------------------------------------------------------------------
// <copyright file="BitTorrentExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.BitTorrent.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.Bttcscan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.BitTorrent.Extensions
{
    /// <summary>
    /// BitTorrent extension methods.
    /// </summary>
    public static class BitTorrentExtensions
    {
        /// <summary>
        /// Add BitTorrent blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithBitTorrentBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IBitTorrentServiceRegistrar, new()
        {
            return optionsBuilder
                .With<BitTorrentAPISettings, TServiceRegistrar>();
        }
    }
}