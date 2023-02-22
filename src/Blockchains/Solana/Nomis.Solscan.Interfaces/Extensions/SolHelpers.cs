// ------------------------------------------------------------------------------------------------------
// <copyright file="SolHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nomis.Solscan.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for solana.
    /// </summary>
    public static class SolHelpers
    {
        /// <summary>
        /// Convert Lamports value to Sol.
        /// </summary>
        /// <param name="valueInLamports">Lamports.</param>
        /// <returns>Returns total Sol.</returns>
        public static decimal ToSol(this long valueInLamports)
        {
            return new BigInteger(valueInLamports).ToSol();
        }

        /// <summary>
        /// Convert Lamports value to Sol.
        /// </summary>
        /// <param name="valueInLamports">Lamports.</param>
        /// <returns>Returns total Sol.</returns>
        public static decimal ToSol(this in BigInteger valueInLamports)
        {
            if (valueInLamports > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInLamports / new BigInteger(100_000_000));
            }

            return (decimal)valueInLamports * 0.000_000_001M;
        }
    }
}