using JWT.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Middlewares;
using UserAPI.Models;
using UserAPI.Services.Interfaces;
using static UserAPI.Errors.Error;

namespace UserAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public ActionResult<ApiResponse<UserModel, object?>> Register([FromBody] RegisterRequest register)
        {
            try
            {
                RegisterServiceResponse result = _authService.Register(register.email, register.password);

                // Set token to cookies
                Response.Cookies.Append("access_token",
                    result.access_token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        MaxAge = TimeSpan.FromMinutes(50026068),
                        Path = "/",
                    });

                Response.Cookies.Append("refresh_token",
                    result.refresh_token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        MaxAge = TimeSpan.FromMinutes(50026068),
                        Path = "/"
                    });

                return CreatedAtAction("Register", new ApiResponse<UserModel, object?>(result.user));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("login")]
        public ActionResult<ApiResponse<UserModel, object?>> Login([FromBody] LoginRequest login)
        {
            try
            {
                LoginServiceResponse response = _authService.Login(login.email, login.password);

                // Set token to cookies
                Response.Cookies.Append("access_token",
                    response.access_token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        MaxAge = TimeSpan.FromMinutes(50026068),
                        Path = "/",
                    });

                Response.Cookies.Append("refresh_token",
                    response.refresh_token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        MaxAge = TimeSpan.FromMinutes(50026068),
                        Path = "/"
                    });

                return Ok(new ApiResponse<UserModel, object?>(response.user));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                string? accessToken = Request.Cookies["access_token"];
                string? refreshToken = Request.Cookies["refresh_token"];

                Response.Cookies.Delete("access_token", new CookieOptions { SameSite = SameSiteMode.None, Secure = true});
                Response.Cookies.Delete("refresh_token", new CookieOptions { SameSite = SameSiteMode.None, Secure = true});

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [VerifyToken]
        [HttpPatch("password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest changePassword)
        {
            try
            {
                _authService.ChangePassword(changePassword.email, changePassword.password, changePassword.new_password);

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("refresh")]
        public IActionResult RefreshToken()
        {
            try
            {
                string? refreshToken = Request.Cookies["refresh_token"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    throw new UnauthorizedError("Không tìm thấy token refresh");
                }

                (string newAccessToken, string newRefreshToken) = _authService.RefreshToken(refreshToken);

                // Set token to cookies
                Response.Cookies.Append("access_token",
                    newAccessToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        MaxAge = TimeSpan.FromMinutes(50026068),
                        Path = "/",
                    });

                Response.Cookies.Append("refresh_token",
                    newRefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        MaxAge = TimeSpan.FromMinutes(50026068),
                        Path = "/"
                    });

                return NoContent();
            }
            catch (Exception ex)
            {
                // clear cookies
                Response.Cookies.Delete("access_token", new CookieOptions { SameSite = SameSiteMode.None, Secure = true});
                Response.Cookies.Delete("refresh_token", new CookieOptions { SameSite = SameSiteMode.None, Secure = true});

                throw;
            }
        }
    }
}
