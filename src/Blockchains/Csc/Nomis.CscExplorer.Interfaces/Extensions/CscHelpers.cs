// ------------------------------------------------------------------------------------------------------
// <copyright file="CscHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.CscExplorer.Interfaces.Models;

namespace Nomis.CscExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for CSC.
    /// </summary>
    public static class CscHelpers
    {
        /// <summary>
        /// Get token UID based on it Contract address and Id.
        /// </summary>
        /// <param name="token">Token info.</param>
        /// <returns>Returns token UID.</returns>
        public static string GetTokenUid(this CscExplorerAccountCrc721TransferRecord token)
        {
            return token.TokenInfo?.Contract + "_" + token.TokenId;
        }
    }
}