// ------------------------------------------------------------------------------------------------------
// <copyright file="SongbirdController.cs" company="Nomis">
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
using Nomis.SongbirdExplorer.Interfaces;
using Nomis.SongbirdExplorer.Interfaces.Models;
using Nomis.SongbirdExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Songbird
{
    /// <summary>
    /// A controller to aggregate all Songbird-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Songbird blockchain.")]
    public sealed class SongbirdController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/songbird";

        /// <summary>
        /// Common tag for Songbird actions.
        /// </summary>
        internal const string SongbirdTag = "Songbird";

        private readonly ILogger<SongbirdController> _logger;
        private readonly ISongbirdScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="SongbirdController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="ISongbirdScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public SongbirdController(
            ISongbirdScoringService scoringService,
            ILogger<SongbirdController> logger)
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
        ///     GET /api/v1/songbird/wallet/0xdde49343a309941ca44ea257bA3c897B47B6b1D4/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetSongbirdWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetSongbirdWalletScore",
            Tags = new[] { SongbirdTag })]
        [ProducesResponseType(typeof(Result<SongbirdWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetSongbirdWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] SongbirdWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<SongbirdWalletStatsRequest, SongbirdWalletScore, SongbirdWalletStats, SongbirdTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}