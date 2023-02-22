// ------------------------------------------------------------------------------------------------------
// <copyright file="Tronscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Tronscan.Extensions;
using Nomis.Tronscan.Interfaces;

namespace Nomis.Tronscan
{
    /// <summary>
    /// Tronscan service registrar.
    /// </summary>
    public sealed class Tronscan :
        ITronServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddTronscanService();
        }
    }
}