using Microsoft.AspNetCore.Mvc;
using MoneyManager.WebApi.Models.Auth;
using MoneyManager.Application.Services.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Npgsql;

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
            var token = await _authService.Login(loginData.UserName, loginData.Password);

            if (string.IsNullOrEmpty(loginData.Password))
                return Ok(new { passwordChangeRequired = true });

            if (token == null)
                return Unauthorized();
            return Ok(new { token });
        }


        [HttpPost(nameof(ChangePassword))]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordData)
        {
            var changed = await _authService.ChangePassword(changePasswordData.UserName, changePasswordData.CurrentPassword, 
                changePasswordData.NewPassword);

            if (changed)
            {
                var token = await _authService.Login(changePasswordData.UserName, changePasswordData.NewPassword);
                return Ok(new { token });
            }
           
            return BadRequest("Password change failed.");
        }
    }
}