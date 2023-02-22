// ------------------------------------------------------------------------------------------------------
// <copyright file="MantleExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Mantle.Settings;
using Nomis.MantleExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Mantle.Extensions
{
    /// <summary>
    /// Mantle extension methods.
    /// </summary>
    public static class MantleExtensions
    {
        /// <summary>
        /// Add Mantle blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithMantleBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IMantleServiceRegistrar, new()
        {
            return optionsBuilder
                .With<MantleAPISettings, TServiceRegistrar>();
        }
    }
}