// ------------------------------------------------------------------------------------------------------
// <copyright file="HederaMirrorNode.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.HederaMirrorNode.Extensions;
using Nomis.HederaMirrorNode.Interfaces;

namespace Nomis.HederaMirrorNode
{
    /// <summary>
    /// HederaMirrorNode service registrar.
    /// </summary>
    public sealed class HederaMirrorNode :
        IHederaServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddHederaMirrorNodeService();
        }
    }
}