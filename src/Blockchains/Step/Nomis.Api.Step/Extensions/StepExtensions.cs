// ------------------------------------------------------------------------------------------------------
// <copyright file="StepExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Step.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.StepScan.Interfaces;

namespace Nomis.Api.Step.Extensions
{
    /// <summary>
    /// Step extension methods.
    /// </summary>
    public static class StepExtensions
    {
        /// <summary>
        /// Add Step blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithStepBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IStepServiceRegistrar, new()
        {
            return optionsBuilder
                .With<StepAPISettings, TServiceRegistrar>();
        }
    }
}