// ------------------------------------------------------------------------------------------------------
// <copyright file="MetisController.cs" company="Nomis">
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
using Nomis.AndromedaExplorer.Interfaces;
using Nomis.AndromedaExplorer.Interfaces.Models;
using Nomis.AndromedaExplorer.Interfaces.Requests;
using Nomis.Api.Common.Swagger.Examples;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Metis
{
    /// <summary>
    /// A controller to aggregate all Metis-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Metis blockchain.")]
    public sealed class MetisController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/metis-andromeda";

        /// <summary>
        /// Common tag for Metis actions.
        /// </summary>
        internal const string MetisTag = "Metis";

        private readonly ILogger<MetisController> _logger;
        private readonly IMetisScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="MetisController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IMetisScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public MetisController(
            IMetisScoringService scoringService,
            ILogger<MetisController> logger)
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
        ///     GET /api/v1/metis-andromeda/wallet/0x7b5B29c949560593977d8AA8c9a5Bc8aB3Eb2709/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetMetisWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetMetisWalletScore",
            Tags = new[] { MetisTag })]
        [ProducesResponseType(typeof(Result<MetisWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetMetisWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] MetisWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<MetisWalletStatsRequest, MetisWalletScore, MetisWalletStats, MetisTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}