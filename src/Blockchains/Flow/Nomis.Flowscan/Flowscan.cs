// ------------------------------------------------------------------------------------------------------
// <copyright file="Flowscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Flowscan.Extensions;
using Nomis.Flowscan.Interfaces;

namespace Nomis.Flowscan
{
    /// <summary>
    /// Flowscan service registrar.
    /// </summary>
    public sealed class Flowscan :
        IFlowServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddFlowscanService();
        }
    }
}