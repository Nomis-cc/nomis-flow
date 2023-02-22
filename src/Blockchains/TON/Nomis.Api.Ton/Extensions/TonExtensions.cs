// ------------------------------------------------------------------------------------------------------
// <copyright file="TonExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Ton.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.Tonscan.Interfaces;

namespace Nomis.Api.Ton.Extensions
{
    /// <summary>
    /// Ton extension methods.
    /// </summary>
    public static class TonExtensions
    {
        /// <summary>
        /// Add Ton blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithTonBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ITonServiceRegistrar, new()
        {
            return optionsBuilder
                .With<TonAPISettings, TServiceRegistrar>();
        }
    }
}