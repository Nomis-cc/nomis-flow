// ------------------------------------------------------------------------------------------------------
// <copyright file="HaqqAPISettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Contracts.Common;

namespace Nomis.Api.Haqq.Settings
{
    /// <summary>
    /// HAQQ API settings.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class HaqqAPISettings :
        IApiSettings
    {
        /// <inheritdoc/>
        public bool APIEnabled { get; set; }

        /// <inheritdoc/>
        public string APIName => HaqqController.HaqqTag;

        /// <inheritdoc/>
        public string ControllerName => nameof(HaqqController);
    }
}