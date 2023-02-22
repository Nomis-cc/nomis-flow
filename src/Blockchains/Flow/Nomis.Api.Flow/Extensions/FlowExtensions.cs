// ------------------------------------------------------------------------------------------------------
// <copyright file="FlowExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.Flow.Settings;
using Nomis.Flowscan.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Flow.Extensions
{
    /// <summary>
    /// Flow extension methods.
    /// </summary>
    public static class FlowExtensions
    {
        /// <summary>
        /// Add Flow blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithFlowBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IFlowServiceRegistrar, new()
        {
            return optionsBuilder
                .With<FlowAPISettings, TServiceRegistrar>();
        }
    }
}