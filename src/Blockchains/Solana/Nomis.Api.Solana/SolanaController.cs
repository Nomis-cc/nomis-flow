// ------------------------------------------------------------------------------------------------------
// <copyright file="SolanaController.cs" company="Nomis">
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
using Nomis.Solscan.Interfaces;
using Nomis.Solscan.Interfaces.Models;
using Nomis.Solscan.Interfaces.Requests;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Solana
{
    /// <summary>
    /// A controller to aggregate all Solana-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Solana blockchain.")]
    public sealed class SolanaController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/solana";

        /// <summary>
        /// Common tag for Solana actions.
        /// </summary>
        internal const string SolanaTag = "Solana";

        private readonly ILogger<SolanaController> _logger;
        private readonly ISolanaScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="SolanaController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="ISolanaScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public SolanaController(
            ISolanaScoringService scoringService,
            ILogger<SolanaController> logger)
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
        ///     GET /api/v1/solana/wallet/8E45wZBrce5NRZiyeUpxF5cAEq72YUewV3cSMepopt8V/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetSolanaWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetSolanaWalletScore",
            Tags = new[] { SolanaTag })]
        [ProducesResponseType(typeof(Result<SolanaWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetSolanaWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] SolanaWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<SolanaWalletStatsRequest, SolanaWalletScore, SolanaWalletStats, SolanaTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get Nomis Score for given wallet addresses.
        /// </summary>
        /// <param name="addresses">List of solana wallet addresses.</param>
        /// <param name="nonce" example="0">Contract nonce.</param>
        /// <param name="deadline" example="0">Verifying deadline block.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>An Nomis Score values and corresponding statistical data.</returns>
        /// <response code="200">Returns Nomis Scores and stats.</response>
        /// <response code="400">Addresses not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpPost("wallets/score", Name = "GetSolanaWalletsScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetSolanaWalletsScore",
            Tags = new[] { SolanaTag })]
        [ProducesResponseType(typeof(Result<List<SolanaWalletScore>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetSolanaWalletsScoreAsync(
            [Required(ErrorMessage = "Wallet addresses should be set"), FromBody] IList<string> addresses,
            ulong nonce = 0,
            ulong deadline = 0,
            CancellationToken cancellationToken = default)
        {
            var result = await _scoringService.GetWalletsStatsAsync(addresses, nonce, deadline, cancellationToken);
            return Ok(result);
        }
    }
}