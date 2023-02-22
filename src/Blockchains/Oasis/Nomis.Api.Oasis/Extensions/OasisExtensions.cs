// ------------------------------------------------------------------------------------------------------
// <copyright file="OasisExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Oasis.Settings;
using Nomis.OasisExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Oasis.Extensions
{
    /// <summary>
    /// Oasis extension methods.
    /// </summary>
    public static class OasisExtensions
    {
        /// <summary>
        /// Add Oasis blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithOasisEmeraldBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IOasisServiceRegistrar, new()
        {
            return optionsBuilder
                .With<OasisAPISettings, TServiceRegistrar>();
        }
    }
}