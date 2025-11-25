using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DTO.Request;
using RealEstate.DTO.Response;
using RealEstate.DTO.ServiceResponse;
using RealEstate.Models;
using RealEstate.Services.Interfaces;
using static RealEstate.Errors.Error;

namespace RealEstate.Controllers
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
                RegisterServiceResponse result = _authService.Register(register.Email, register.Password);

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
                LoginServiceResponse response = _authService.Login(login.Email, login.Password);

                return Ok(new ApiResponse<UserModel, MetaToken>(response.user, response.meta));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
