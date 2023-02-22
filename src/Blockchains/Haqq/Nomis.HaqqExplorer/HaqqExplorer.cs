// ------------------------------------------------------------------------------------------------------
// <copyright file="HaqqExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.HaqqExplorer.Extensions;
using Nomis.HaqqExplorer.Interfaces;

namespace Nomis.HaqqExplorer
{
    /// <summary>
    /// HAQQ Explorer service registrar.
    /// </summary>
    public sealed class HaqqExplorer :
        IHaqqServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddHaqqExplorerService();
        }
    }
}