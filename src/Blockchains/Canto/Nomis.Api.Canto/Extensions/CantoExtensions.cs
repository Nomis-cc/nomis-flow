// ------------------------------------------------------------------------------------------------------
// <copyright file="CantoExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Canto.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.CantoExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Canto.Extensions
{
    /// <summary>
    /// Canto extension methods.
    /// </summary>
    public static class CantoExtensions
    {
        /// <summary>
        /// Add Canto blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithCantoBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ICantoServiceRegistrar, new()
        {
            return optionsBuilder
                .With<CantoAPISettings, TServiceRegistrar>();
        }
    }
}