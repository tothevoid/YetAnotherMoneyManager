#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Auth;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.WebApi.Models.Auth;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private const string RefreshTokenCookieKey = "refreshToken";
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel loginData)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            var result = await _authService.LoginAsync(loginData.UserName, loginData.Password, ipAddress, userAgent);

            if (result.PasswordChangeRequired)
            {
                return Ok(result);
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenCookie(result.RefreshToken, DateTime.UtcNow.AddDays(30));
            }

            return Ok(result);
        }

        [HttpPost(nameof(RefreshToken))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto? request)
        {
            var refreshToken = request?.RefreshToken;
            if (string.IsNullOrEmpty(refreshToken) && Request.Cookies.TryGetValue(RefreshTokenCookieKey, out var cookieToken))
            {
                refreshToken = cookieToken;
            }

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "Refresh token is missing." });
            }

            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers.UserAgent.ToString();

                var result = await _authService.RefreshTokenAsync(refreshToken, ipAddress, userAgent);
                SetRefreshTokenCookie(result.RefreshToken, result.ExpiresAt);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ClearRefreshTokenCookie();
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost(nameof(RevokeToken))]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDto? request)
        {
            var refreshToken = request?.RefreshToken;
            if (string.IsNullOrEmpty(refreshToken) && Request.Cookies.TryGetValue(RefreshTokenCookieKey, out var cookieToken))
            {
                refreshToken = cookieToken;
            }

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.RevokeTokenAsync(refreshToken);
            }

            ClearRefreshTokenCookie();
            return Ok(new { message = "Token revoked." });
        }

        [Authorize]
        [HttpPost(nameof(RevokeAll))]
        public async Task<IActionResult> RevokeAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                await _authService.RevokeAllUserTokensAsync(userId);
            }

            ClearRefreshTokenCookie();
            return Ok(new { message = "All sessions revoked." });
        }

        [HttpPost(nameof(ChangePassword))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordData)
        {
            var changed = await _authService.ChangePasswordAsync(
                changePasswordData.UserName,
                changePasswordData.CurrentPassword,
                changePasswordData.NewPassword);

            if (changed)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers.UserAgent.ToString();

                var result = await _authService.LoginAsync(
                    changePasswordData.UserName,
                    changePasswordData.NewPassword,
                    ipAddress,
                    userAgent);

                if (!string.IsNullOrEmpty(result.RefreshToken))
                {
                    SetRefreshTokenCookie(result.RefreshToken, DateTime.UtcNow.AddDays(30));
                }

                return Ok(result);
            }

            return BadRequest("Password change failed.");
        }

        private void SetRefreshTokenCookie(string refreshToken, DateTime expiresAt)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = expiresAt,
                Path = "/Auth"
            };

            Response.Cookies.Append(RefreshTokenCookieKey, refreshToken, cookieOptions);
        }

        private void ClearRefreshTokenCookie()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Path = "/Auth"
            };

            Response.Cookies.Delete(RefreshTokenCookieKey, cookieOptions);
        }
    }
}