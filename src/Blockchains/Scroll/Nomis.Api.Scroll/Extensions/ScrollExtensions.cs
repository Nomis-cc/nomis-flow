// ------------------------------------------------------------------------------------------------------
// <copyright file="ScrollExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Scroll.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.ScrollScan.Interfaces;

namespace Nomis.Api.Scroll.Extensions
{
    /// <summary>
    /// Scroll extension methods.
    /// </summary>
    public static class ScrollExtensions
    {
        /// <summary>
        /// Add Scroll blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithScrollBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IScrollServiceRegistrar, new()
        {
            return optionsBuilder
                .With<ScrollAPISettings, TServiceRegistrar>();
        }
    }
}