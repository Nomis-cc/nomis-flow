// ------------------------------------------------------------------------------------------------------
// <copyright file="AptosController.cs" company="Nomis">
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
using Nomis.AptoslabsExplorer.Interfaces;
using Nomis.AptoslabsExplorer.Interfaces.Models;
using Nomis.AptoslabsExplorer.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Aptos
{
    /// <summary>
    /// A controller to aggregate all Aptos-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Aptos blockchain.")]
    public sealed class AptosController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/aptos";

        /// <summary>
        /// Common tag for Aptos actions.
        /// </summary>
        internal const string AptosTag = "Aptos";

        private readonly ILogger<AptosController> _logger;
        private readonly IAptosScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="AptosController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IAptosScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public AptosController(
            IAptosScoringService scoringService,
            ILogger<AptosController> logger)
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
        ///     GET /api/v1/aptos/wallet/0x8fbb89b979c0a6976e77be646a421a4d40107c20c896b4d11af40d7a44ee1e01/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetAptosWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetAptosWalletScore",
            Tags = new[] { AptosTag })]
        [ProducesResponseType(typeof(Result<AptosWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAptosWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] AptosWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<AptosWalletStatsRequest, AptosWalletScore, AptosWalletStats, AptosTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}