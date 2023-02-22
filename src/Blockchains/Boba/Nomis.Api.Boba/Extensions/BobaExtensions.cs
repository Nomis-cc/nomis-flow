// ------------------------------------------------------------------------------------------------------
// <copyright file="BobaExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Boba.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.Bobascan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Boba.Extensions
{
    /// <summary>
    /// Boba extension methods.
    /// </summary>
    public static class BobaExtensions
    {
        /// <summary>
        /// Add Boba blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithBobaBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IBobaServiceRegistrar, new()
        {
            return optionsBuilder
                .With<BobaAPISettings, TServiceRegistrar>();
        }
    }
}