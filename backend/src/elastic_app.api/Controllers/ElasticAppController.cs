using elastic_app.application.DTOs;
using elastic_app.application.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace elastic_app.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElasticAppController : ControllerBase
    {
        private readonly IUserService _userService;

        public ElasticAppController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registrationDetails)
        {
            try
            {
                await _userService.RegisterUserAsync(registrationDetails);

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

    }
}
