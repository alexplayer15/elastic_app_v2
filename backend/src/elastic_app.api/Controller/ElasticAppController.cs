using MediatR;
using Microsoft.AspNetCore.Mvc;
using elastic_app.application.Commands;

namespace elastic_app.api.Controller
{
    [Route("api")]
    [ApiController]
    public class ElasticAppController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ElasticAppController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestCommand registerRequest)
        {
            try
            {
                await _mediator.Send(registerRequest);

                return Ok(new { message = "Registration successful." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            try
            {
                await _mediator.Send(new EmailVerificationCommand(token));

                return Ok(new { message = "Email verified!" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}

