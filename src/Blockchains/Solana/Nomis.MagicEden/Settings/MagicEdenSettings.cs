// ------------------------------------------------------------------------------------------------------
// <copyright file="MagicEdenSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Contracts.Common;

namespace Nomis.MagicEden.Settings
{
    /// <summary>
    /// MagicEden settings.
    /// </summary>
    internal class MagicEdenSettings :
        ISettings
    {
        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://api.magiceden.dev"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }
    }
}