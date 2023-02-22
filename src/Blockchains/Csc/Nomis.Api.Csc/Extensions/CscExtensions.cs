// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Csc.Settings;
using Nomis.CscExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Csc.Extensions
{
    /// <summary>
    /// Csc extension methods.
    /// </summary>
    public static class CscExtensions
    {
        /// <summary>
        /// Add CSC blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithCSCBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ICscServiceRegistrar, new()
        {
            return optionsBuilder
                .With<CscAPISettings, TServiceRegistrar>();
        }
    }
}