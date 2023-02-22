// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonriverMoonscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.MoonriverMoonscan.Extensions;
using Nomis.MoonriverMoonscan.Interfaces;

namespace Nomis.MoonriverMoonscan
{
    /// <summary>
    /// Moonriver moonscan service registrar.
    /// </summary>
    public sealed class MoonriverMoonscan :
        IMoonriverServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddMoonriverMoonscanService();
        }
    }
}