using Microsoft.AspNetCore.Mvc;
using MoneyManager.WebApi.Models.Auth;
using MoneyManager.Application.Services.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.LoginAsync(model.Username, model.Password);
            if (token == null)
                return Unauthorized();
            return Ok(new { token });
        }
    }
}