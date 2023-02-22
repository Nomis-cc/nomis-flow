// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonbeamExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Moonbeam.Settings;
using Nomis.Moonscan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Moonbeam.Extensions
{
    /// <summary>
    /// Moonbeam extension methods.
    /// </summary>
    public static class MoonbeamExtensions
    {
        /// <summary>
        /// Add Moonbeam blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithMoonbeamBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IMoonbeamServiceRegistrar, new()
        {
            return optionsBuilder
                .With<MoonbeamAPISettings, TServiceRegistrar>();
        }
    }
}