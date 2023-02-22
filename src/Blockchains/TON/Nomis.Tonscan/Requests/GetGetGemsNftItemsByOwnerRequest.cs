// ------------------------------------------------------------------------------------------------------
// <copyright file="GetGetGemsNftItemsByOwnerRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Tonscan.Requests
{
    /// <summary>
    /// Request for getting the GetGems non fungible tokens by owner.
    /// </summary>
    public class GetGetGemsNftItemsByOwnerRequest
    {
        /// <summary>
        /// Initialize <see cref="GetGetGemsNftItemsByOwnerRequest"/>
        /// </summary>
        /// <param name="ownerAddress">Owner address.</param>
        /// <param name="first">First.</param>
        /// <param name="after">After.</param>
        public GetGetGemsNftItemsByOwnerRequest(
            string ownerAddress,
            int first = 10000,
            string? after = null)
        {
            OwnerAddress = ownerAddress;
            First = first;
            After = after;
        }

        /// <summary>
        /// Owner address.
        /// </summary>
        /// <example>EQBfAN7LfaUYgXZNw5Wc7GBgkEX2yhuJ5ka95J1JJwXXf4a8</example>
        public string? OwnerAddress { get; }

        /// <summary>
        /// First.
        /// </summary>
        /// <example>10000</example>
        public int First { get; }

        /// <summary>
        /// After.
        /// </summary>
        /// <example>null</example>
        public string? After { get; }
    }
}