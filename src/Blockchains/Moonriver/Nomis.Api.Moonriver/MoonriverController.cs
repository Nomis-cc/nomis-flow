// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonriverController.cs" company="Nomis">
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
using Nomis.MoonriverMoonscan.Interfaces;
using Nomis.MoonriverMoonscan.Interfaces.Models;
using Nomis.MoonriverMoonscan.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Moonriver
{
    /// <summary>
    /// A controller to aggregate all Moonriver-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Moonriver blockchain.")]
    public sealed class MoonriverController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/moonriver";

        /// <summary>
        /// Common tag for Moonriver actions.
        /// </summary>
        internal const string MoonriverTag = "Moonriver";

        private readonly ILogger<MoonriverController> _logger;
        private readonly IMoonriverScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="MoonriverController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IMoonriverScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public MoonriverController(
            IMoonriverScoringService scoringService,
            ILogger<MoonriverController> logger)
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
        ///     GET /api/v1/moonriver/wallet/0xd617CC373ECf63c592B44CA76290290bB7321c9F/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetMoonriverWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetMoonriverWalletScore",
            Tags = new[] { MoonriverTag })]
        [ProducesResponseType(typeof(Result<MoonriverWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetMoonriverWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] MoonriverWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<MoonriverWalletStatsRequest, MoonriverWalletScore, MoonriverWalletStats, MoonriverTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}