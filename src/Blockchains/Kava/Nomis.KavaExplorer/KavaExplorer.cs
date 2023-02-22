// ------------------------------------------------------------------------------------------------------
// <copyright file="KavaExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.KavaExplorer.Extensions;
using Nomis.KavaExplorer.Interfaces;

namespace Nomis.KavaExplorer
{
    /// <summary>
    /// Kava Explorer service registrar.
    /// </summary>
    public sealed class KavaExplorer :
        IKavaServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddKavaExplorerService();
        }
    }
}