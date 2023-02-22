// ------------------------------------------------------------------------------------------------------
// <copyright file="Xrpscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Xrpscan.Extensions;
using Nomis.Xrpscan.Interfaces;

namespace Nomis.Xrpscan
{
    /// <summary>
    /// Xrpscan service registrar.
    /// </summary>
    public sealed class Xrpscan :
        IRippleServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddXrpscanService();
        }
    }
}