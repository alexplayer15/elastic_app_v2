using elastic_app.api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace elastic_app.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElasticAppController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            return Ok();
        }

    }
}
