// ------------------------------------------------------------------------------------------------------
// <copyright file="GetFlowscanTransactionsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Flowscan.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the Flowscan transactions.
    /// </summary>
    public class GetFlowscanTransactionsRequest
    {
        /// <summary>
        /// Initialize <see cref="GetFlowscanTransactionsRequest"/>
        /// </summary>
        /// <param name="accountId">Account id.</param>
        /// <param name="after">After.</param>
        public GetFlowscanTransactionsRequest(
            string accountId,
            string? after = null)
        {
            AccountId = accountId;
            After = after;
        }

        /// <summary>
        /// Account id.
        /// </summary>
        /// <example>0x6394e988297f5ed2</example>
        public string? AccountId { get; set; }

        /// <summary>
        /// After.
        /// </summary>
        /// <example>MjY1OTM1NDMvMQ==</example>
        public string? After { get; set; }
    }
}