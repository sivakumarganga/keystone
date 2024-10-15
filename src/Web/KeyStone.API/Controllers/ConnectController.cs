using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeyStone.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/Connect")]
    public class ConnectController : BaseController
    {
        [HttpPost("Register")]
        public IActionResult Register()
        {
            return Ok("Register");
        }

        [HttpPost("Token")]
        public IActionResult Token()
        {
            return Ok("Token");
        }

        [HttpPost("RefreshSignIn")]
        public IActionResult RefreshSignIn()
        {
            return Ok("RefreshSignIn");
        }

        [Authorize]
        [HttpGet("UserInfo")]
        public IActionResult UserInfo()
        {
            return Ok("UserInfo");
        }

        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok("Logout");
        }
    }
}
