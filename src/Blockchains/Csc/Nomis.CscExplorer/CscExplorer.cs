// ------------------------------------------------------------------------------------------------------
// <copyright file="CscExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.CscExplorer.Extensions;
using Nomis.CscExplorer.Interfaces;

namespace Nomis.CscExplorer
{
    /// <summary>
    /// CscExplorer service registrar.
    /// </summary>
    public sealed class CscExplorer :
        ICscServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddCscExplorerService();
        }
    }
}