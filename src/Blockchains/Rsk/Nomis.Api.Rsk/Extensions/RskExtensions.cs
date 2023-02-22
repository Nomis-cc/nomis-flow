// ------------------------------------------------------------------------------------------------------
// <copyright file="RskExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Rsk.Settings;
using Nomis.RskExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Rsk.Extensions
{
    /// <summary>
    /// Rsk extension methods.
    /// </summary>
    public static class RskExtensions
    {
        /// <summary>
        /// Add RSK blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithRSKBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IRskServiceRegistrar, new()
        {
            return optionsBuilder
                .With<RskAPISettings, TServiceRegistrar>();
        }
    }
}