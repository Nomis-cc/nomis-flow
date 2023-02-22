// ------------------------------------------------------------------------------------------------------
// <copyright file="Solscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.MagicEden.Extensions;
using Nomis.Solscan.Extensions;
using Nomis.Solscan.Interfaces;

namespace Nomis.Solscan
{
    /// <summary>
    /// Solscan service registrar.
    /// </summary>
    public sealed class Solscan :
        ISolanaServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddMagicEdenService()
                .AddSolscanService();
        }
    }
}