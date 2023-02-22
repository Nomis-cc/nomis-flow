// ------------------------------------------------------------------------------------------------------
// <copyright file="SolanaAPISettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Contracts.Common;

namespace Nomis.Api.Solana.Settings
{
    /// <summary>
    /// Solana API settings.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class SolanaAPISettings :
        IApiSettings
    {
        /// <inheritdoc/>
        public bool APIEnabled { get; set; }

        /// <inheritdoc/>
        public string APIName => SolanaController.SolanaTag;

        /// <inheritdoc/>
        public string ControllerName => nameof(SolanaController);
    }
}