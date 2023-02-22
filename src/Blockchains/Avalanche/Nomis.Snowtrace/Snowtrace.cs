// ------------------------------------------------------------------------------------------------------
// <copyright file="Snowtrace.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Snowtrace.Extensions;
using Nomis.Snowtrace.Interfaces;

namespace Nomis.Snowtrace
{
    /// <summary>
    /// Snowtrace service registrar.
    /// </summary>
    public sealed class Snowtrace :
        IAvalancheServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddSnowtraceService();
        }
    }
}