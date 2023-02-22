// ------------------------------------------------------------------------------------------------------
// <copyright file="BscController.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomis.Api.Common.Swagger.Examples;
using Nomis.Bscscan.Interfaces;
using Nomis.Bscscan.Interfaces.Models;
using Nomis.Bscscan.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Bsc
{
    /// <summary>
    /// A controller to aggregate all BSC-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Binance Smart Chain blockchain.")]
    public sealed class BscController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/bsc";

        /// <summary>
        /// Common tag for BSC actions.
        /// </summary>
        internal const string BscTag = "Bsc";

        private readonly ILogger<BscController> _logger;
        private readonly IBscScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="BscController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IBscScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public BscController(
            IBscScoringService scoringService,
            ILogger<BscController> logger)
        {
            _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get Nomis Score for given wallet address.
        /// </summary>
        /// <param name="request">Request for getting the wallet stats.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>An Nomis Score value and corresponding statistical data.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/bsc/wallet/0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetBscWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetBscWalletScore",
            Tags = new[] { BscTag })]
        [ProducesResponseType(typeof(Result<BscWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetBscWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] BscWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<BscWalletStatsRequest, BscWalletScore, BscWalletStats, BscTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}