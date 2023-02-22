// ------------------------------------------------------------------------------------------------------
// <copyright file="Bobascan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Bobascan.Extensions;
using Nomis.Bobascan.Interfaces;

namespace Nomis.Bobascan
{
    /// <summary>
    /// Bobascan Explorer service registrar.
    /// </summary>
    public sealed class Bobascan :
        IBobaServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddBobascanService();
        }
    }
}