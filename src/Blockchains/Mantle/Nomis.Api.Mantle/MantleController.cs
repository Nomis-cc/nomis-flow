// ------------------------------------------------------------------------------------------------------
// <copyright file="MantleController.cs" company="Nomis">
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
using Nomis.MantleExplorer.Interfaces;
using Nomis.MantleExplorer.Interfaces.Models;
using Nomis.MantleExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Mantle
{
    /// <summary>
    /// A controller to aggregate all Mantle-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Mantle Testnet blockchain.")]
    public sealed class MantleController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/mantle-testnet";

        /// <summary>
        /// Common tag for Mantle actions.
        /// </summary>
        internal const string MantleTag = "Mantle";

        private readonly ILogger<MantleController> _logger;
        private readonly IMantleScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="MantleController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IMantleScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public MantleController(
            IMantleScoringService scoringService,
            ILogger<MantleController> logger)
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
        ///     GET /api/v1/mantle-testnet/wallet/0xBDdd3A99Fb14d586BD7B261634CC2AbF7D51e7dC/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetMantleWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetMantleWalletScore",
            Tags = new[] { MantleTag })]
        [ProducesResponseType(typeof(Result<MantleWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetMantleWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] MantleWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<MantleWalletStatsRequest, MantleWalletScore, MantleWalletStats, MantleTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}