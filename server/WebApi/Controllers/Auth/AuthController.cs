using Microsoft.AspNetCore.Mvc;
using MoneyManager.WebApi.Models.Auth;
using MoneyManager.Application.Services.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginModel loginData)
        {
            var token = await _authService.LoginAsync(loginData.UserName, loginData.Password);
            if (token == null)
                return Unauthorized();
            return Ok(new { token });
        }
    }
}