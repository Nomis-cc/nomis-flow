// ------------------------------------------------------------------------------------------------------
// <copyright file="EvmosAPI.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.EvmosAPI.Extensions;
using Nomis.EvmosAPI.Interfaces;

namespace Nomis.EvmosAPI
{
    /// <summary>
    /// EvmosAPI service registrar.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class EvmosAPI :
        IEvmosServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddEvmosAPIService();
        }
    }
}