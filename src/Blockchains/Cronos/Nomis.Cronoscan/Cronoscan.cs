// ------------------------------------------------------------------------------------------------------
// <copyright file="Cronoscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Cronoscan.Extensions;
using Nomis.Cronoscan.Interfaces;

namespace Nomis.Cronoscan
{
    /// <summary>
    /// Cronoscan service registrar.
    /// </summary>
    public sealed class Cronoscan :
        ICronosServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddCronoscanService();
        }
    }
}