// ------------------------------------------------------------------------------------------------------
// <copyright file="ScrollScan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.ScrollScan.Extensions;
using Nomis.ScrollScan.Interfaces;

namespace Nomis.ScrollScan
{
    /// <summary>
    /// ScrollScan service registrar.
    /// </summary>
    public sealed class ScrollScan :
        IScrollServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddScrollScanService();
        }
    }
}