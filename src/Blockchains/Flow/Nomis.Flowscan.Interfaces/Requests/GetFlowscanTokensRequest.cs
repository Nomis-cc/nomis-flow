// ------------------------------------------------------------------------------------------------------
// <copyright file="GetFlowscanTokensRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Flowscan.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the Flowscan tokens.
    /// </summary>
    public class GetFlowscanTokensRequest
    {
        /// <summary>
        /// Initialize <see cref="GetFlowscanTokensRequest"/>
        /// </summary>
        /// <param name="accountId">Account id.</param>
        public GetFlowscanTokensRequest(
            string accountId)
        {
            AccountId = accountId;
        }

        /// <summary>
        /// Account id.
        /// </summary>
        /// <example>0x6394e988297f5ed2</example>
        public string AccountId { get; }
    }
}