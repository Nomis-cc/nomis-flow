// ------------------------------------------------------------------------------------------------------
// <copyright file="NovaArbiscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.NovaArbiscan.Extensions;
using Nomis.NovaArbiscan.Interfaces;

namespace Nomis.NovaArbiscan
{
    /// <summary>
    /// Nova arbiscan service registrar.
    /// </summary>
    public sealed class NovaArbiscan :
        IArbitrumNovaServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddNovaArbiscanService();
        }
    }
}