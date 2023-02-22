// ------------------------------------------------------------------------------------------------------
// <copyright file="Bttcscan.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Bttcscan.Extensions;
using Nomis.Bttcscan.Interfaces;

namespace Nomis.Bttcscan
{
    /// <summary>
    /// Bttcscan service registrar.
    /// </summary>
    public sealed class Bttcscan :
        IBitTorrentServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddBttcscanService();
        }
    }
}