// ------------------------------------------------------------------------------------------------------
// <copyright file="ShidenExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Shiden.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.ShidenExplorer.Interfaces;

namespace Nomis.Api.Shiden.Extensions
{
    /// <summary>
    /// Shiden extension methods.
    /// </summary>
    public static class ShidenExtensions
    {
        /// <summary>
        /// Add Shiden blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithShidenBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IShidenServiceRegistrar, new()
        {
            return optionsBuilder
                .With<ShidenAPISettings, TServiceRegistrar>();
        }
    }
}