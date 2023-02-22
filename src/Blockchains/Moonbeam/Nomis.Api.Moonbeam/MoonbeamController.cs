// ------------------------------------------------------------------------------------------------------
// <copyright file="MoonbeamController.cs" company="Nomis">
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
using Nomis.Moonscan.Interfaces;
using Nomis.Moonscan.Interfaces.Models;
using Nomis.Moonscan.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Moonbeam
{
    /// <summary>
    /// A controller to aggregate all Moonbeam-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Moonbeam blockchain.")]
    public sealed class MoonbeamController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/moonbeam";

        /// <summary>
        /// Common tag for Moonbeam actions.
        /// </summary>
        internal const string MoonbeamTag = "Moonbeam";

        private readonly ILogger<MoonbeamController> _logger;
        private readonly IMoonbeamScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="MoonbeamController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IMoonbeamScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public MoonbeamController(
            IMoonbeamScoringService scoringService,
            ILogger<MoonbeamController> logger)
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
        ///     GET /api/v1/moonbeam/wallet/0xBb8421f2e9DC8b503711F70e051E19EA4b30F5Cb/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetMoonbeamWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetMoonbeamWalletScore",
            Tags = new[] { MoonbeamTag })]
        [ProducesResponseType(typeof(Result<MoonbeamWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetMoonbeamWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] MoonbeamWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<MoonbeamWalletStatsRequest, MoonbeamWalletScore, MoonbeamWalletStats, MoonbeamTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}