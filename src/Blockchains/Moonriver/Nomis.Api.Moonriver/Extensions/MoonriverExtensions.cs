// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonriverExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Moonriver.Settings;
using Nomis.MoonriverMoonscan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Moonriver.Extensions
{
    /// <summary>
    /// Moonriver extension methods.
    /// </summary>
    public static class MoonriverExtensions
    {
        /// <summary>
        /// Add Moonriver blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithMoonriverBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IMoonriverServiceRegistrar, new()
        {
            return optionsBuilder
                .With<MoonriverAPISettings, TServiceRegistrar>();
        }
    }
}