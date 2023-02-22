// ------------------------------------------------------------------------------------------------------
// <copyright file="DogechainExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.DogechainExplorer.Extensions;
using Nomis.DogechainExplorer.Interfaces;

namespace Nomis.DogechainExplorer
{
    /// <summary>
    /// Dogechain Explorer service registrar.
    /// </summary>
    public sealed class DogechainExplorer :
        IDogechainServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddDogechainExplorerService();
        }
    }
}