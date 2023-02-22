// ------------------------------------------------------------------------------------------------------
// <copyright file="SongbirdExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.SongbirdExplorer.Extensions;
using Nomis.SongbirdExplorer.Interfaces;

namespace Nomis.SongbirdExplorer
{
    /// <summary>
    /// Songbird Explorer service registrar.
    /// </summary>
    public sealed class SongbirdExplorer :
        ISongbirdServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddSongbirdExplorerService();
        }
    }
}