// ------------------------------------------------------------------------------------------------------
// <copyright file="Aurorascan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Aurorascan.Extensions;
using Nomis.Aurorascan.Interfaces;

namespace Nomis.Aurorascan
{
    /// <summary>
    /// Aurorascan service registrar.
    /// </summary>
    public sealed class Aurorascan :
        IAuroraServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddAurorascanService();
        }
    }
}