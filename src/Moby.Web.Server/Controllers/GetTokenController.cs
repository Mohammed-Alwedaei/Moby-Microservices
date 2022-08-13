using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Moby.Web.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GetTokenController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var token = await HttpContext.GetUserAccessTokenAsync();

            return Ok(token);
        }
    }
}