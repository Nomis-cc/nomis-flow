// ------------------------------------------------------------------------------------------------------
// <copyright file="VelasExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Velas.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.VelasExplorer.Interfaces;

namespace Nomis.Api.Velas.Extensions
{
    /// <summary>
    /// Velas extension methods.
    /// </summary>
    public static class VelasExtensions
    {
        /// <summary>
        /// Add Velas blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithVelasBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IVelasServiceRegistrar, new()
        {
            return optionsBuilder
                .With<VelasAPISettings, TServiceRegistrar>();
        }
    }
}