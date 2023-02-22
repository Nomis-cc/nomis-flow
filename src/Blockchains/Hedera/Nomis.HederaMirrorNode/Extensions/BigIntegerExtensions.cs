// ------------------------------------------------------------------------------------------------------
// <copyright file="BigIntegerExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nomis.HederaMirrorNode.Extensions
{
    /// <summary>
    /// Extension methods for converting <see cref="BigInteger"/>.
    /// </summary>
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Get sum of the source items.
        /// </summary>
        /// <typeparam name="TSource">The type of source items.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">Selector function.</param>
        /// <returns>Returns sum of the source items.</returns>
        public static BigInteger Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, BigInteger> selector)
        {
            BigInteger sum = 0;
            foreach (var item in source)
            {
                sum += selector(item);
            }

            return sum;
        }
    }
}