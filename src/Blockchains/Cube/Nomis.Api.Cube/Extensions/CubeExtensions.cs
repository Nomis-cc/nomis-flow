// ------------------------------------------------------------------------------------------------------
// <copyright file="CubeExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Cube.Settings;
using Nomis.Cubescan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Cube.Extensions
{
    /// <summary>
    /// Cube extension methods.
    /// </summary>
    public static class CubeExtensions
    {
        /// <summary>
        /// Add Cube blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithCubeBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ICubeServiceRegistrar, new()
        {
            return optionsBuilder
                .With<CubeAPISettings, TServiceRegistrar>();
        }
    }
}