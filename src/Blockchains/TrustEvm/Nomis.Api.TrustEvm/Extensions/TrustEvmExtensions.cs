// ------------------------------------------------------------------------------------------------------
// <copyright file="TrustEvmExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Common.Extensions;
using Nomis.Api.TrustEvm.Settings;
using Nomis.ScoringService.Interfaces.Builder;
using Nomis.Trustscan.Interfaces;

namespace Nomis.Api.TrustEvm.Extensions
{
    /// <summary>
    /// Trust EVM extension methods.
    /// </summary>
    public static class TrustEvmExtensions
    {
        /// <summary>
        /// Add Trust EVM blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        // ReSharper disable once InconsistentNaming
        public static IScoringOptionsBuilder WithTrustEVMBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : ITrustEvmServiceRegistrar, new()
        {
            return optionsBuilder
                .With<TrustEvmAPISettings, TServiceRegistrar>();
        }
    }
}