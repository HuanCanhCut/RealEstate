using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Register([FromBody] RegisterRequest register)
        {
            try
            {
                UserModel user = _authService.Register(register.Email, register.Password);

                return CreatedAtAction(nameof(Register), user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
