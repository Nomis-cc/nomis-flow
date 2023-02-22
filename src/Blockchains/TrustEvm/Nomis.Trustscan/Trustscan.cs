// ------------------------------------------------------------------------------------------------------
// <copyright file="Trustscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Trustscan.Extensions;
using Nomis.Trustscan.Interfaces;

namespace Nomis.Trustscan
{
    /// <summary>
    /// Trustscan service registrar.
    /// </summary>
    public sealed class Trustscan :
        ITrustEvmServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddTrustscanService();
        }
    }
}