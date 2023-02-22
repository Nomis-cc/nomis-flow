// ------------------------------------------------------------------------------------------------------
// <copyright file="Moonscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Moonscan.Extensions;
using Nomis.Moonscan.Interfaces;

namespace Nomis.Moonscan
{
    /// <summary>
    /// Moonscan service registrar.
    /// </summary>
    public sealed class Moonscan :
        IMoonbeamServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddMoonscanService();
        }
    }
}