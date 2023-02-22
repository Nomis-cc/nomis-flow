// ------------------------------------------------------------------------------------------------------
// <copyright file="Tonscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Tonscan.Extensions;
using Nomis.Tonscan.Interfaces;

namespace Nomis.Tonscan
{
    /// <summary>
    /// Tonscan service registrar.
    /// </summary>
    public sealed class Tonscan :
        ITonServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddTonscanService();
        }
    }
}