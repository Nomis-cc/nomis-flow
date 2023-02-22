// ------------------------------------------------------------------------------------------------------
// <copyright file="AuroraExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Aurora.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.Aurorascan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Aurora.Extensions
{
    /// <summary>
    /// Aurora extension methods.
    /// </summary>
    public static class AuroraExtensions
    {
        /// <summary>
        /// Add Aurora blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithAuroraBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IAuroraServiceRegistrar, new()
        {
            return optionsBuilder
                .With<AuroraAPISettings, TServiceRegistrar>();
        }
    }
}