// ------------------------------------------------------------------------------------------------------
// <copyright file="CronosExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Cronos.Settings;
using Nomis.Cronoscan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Cronos.Extensions
{
    /// <summary>
    /// Cronos extension methods.
    /// </summary>
    public static class CronosExtensions
    {
        /// <summary>
        /// Add Cronos blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithCronosBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ICronosServiceRegistrar, new()
        {
            return optionsBuilder
                .With<CronosAPISettings, TServiceRegistrar>();
        }
    }
}