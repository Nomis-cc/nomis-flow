// ------------------------------------------------------------------------------------------------------
// <copyright file="Klaytnscope.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.Klaytnscope.Extensions;
using Nomis.Klaytnscope.Interfaces;

namespace Nomis.Klaytnscope
{
    /// <summary>
    /// Klaytnscope service registrar.
    /// </summary>
    public sealed class Klaytnscope :
        IKlaytnServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddKlaytnscopeService();
        }
    }
}