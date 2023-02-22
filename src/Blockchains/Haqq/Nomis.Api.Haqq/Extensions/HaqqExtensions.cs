// ------------------------------------------------------------------------------------------------------
// <copyright file="HaqqExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Haqq.Settings;
using Nomis.HaqqExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Haqq.Extensions
{
    /// <summary>
    /// Haqq extension methods.
    /// </summary>
    public static class HaqqExtensions
    {
        /// <summary>
        /// Add HAQQ blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithHAQQBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IHaqqServiceRegistrar, new()
        {
            return optionsBuilder
                .With<HaqqAPISettings, TServiceRegistrar>();
        }
    }
}