using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;
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
        public ActionResult<ApiResponse<UserModel, MetaToken>> Register([FromBody] RegisterRequest register)
        {
            try
            {
                RegisterServiceResponse result = _authService.Register(register.email, register.password);

                return CreatedAtAction("Register", new ApiResponse<UserModel, MetaToken>(result.user, result.meta));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("login")]
        public ActionResult<ApiResponse<UserModel, MetaToken>> Login([FromBody] LoginRequest login)
        {
            try
            {
                LoginServiceResponse response = _authService.Login(login.email, login.password);

                return Ok(new ApiResponse<UserModel, MetaToken>(response.user, response.meta));
            }
            catch (Exception)
            {
                throw;
            }
        }

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
    }
}
