// ------------------------------------------------------------------------------------------------------
// <copyright file="MetisAPISettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Contracts.Common;

namespace Nomis.Api.Metis.Settings
{
    /// <summary>
    /// Metis API settings.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class MetisAPISettings :
        IApiSettings
    {
        /// <inheritdoc/>
        public bool APIEnabled { get; set; }

        /// <inheritdoc/>
        public string APIName => MetisController.MetisTag;

        /// <inheritdoc/>
        public string ControllerName => nameof(MetisController);
    }
}