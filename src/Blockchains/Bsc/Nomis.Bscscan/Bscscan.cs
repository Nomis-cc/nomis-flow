// ------------------------------------------------------------------------------------------------------
// <copyright file="Bscscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Bscscan.Extensions;
using Nomis.Bscscan.Interfaces;

namespace Nomis.Bscscan
{
    /// <summary>
    /// Bscscan service registrar.
    /// </summary>
    public sealed class Bscscan :
        IBscServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddBscscanService();
        }
    }
}