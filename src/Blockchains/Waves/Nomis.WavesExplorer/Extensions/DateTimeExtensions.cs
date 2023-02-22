// ------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Extensions;

namespace Nomis.WavesExplorer.Extensions
{
    /// <summary>
    /// Extension methods for converting DateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert Unix TimeStamp to DateTime.
        /// </summary>
        /// <param name="unixTimeStamp">Unix TimeStamp in string.</param>
        /// <returns><see cref="DateTime"/>.</returns>
        public static DateTime ToWavesDateTime(this string unixTimeStamp)
        {
            string unixTimeStampExactly = string.Join(string.Empty, unixTimeStamp.Take(unixTimeStamp.Length - 3));
            return unixTimeStampExactly.ToDateTime();
        }
    }
}