using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DTO.Response;
using RealEstate.Middlewares;
using RealEstate.Models;
using RealEstate.Services.Interfaces;
using RealEstate.Utils;

namespace RealEstate.Controllers
{
    [VerifyToken]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("me")]
        public ActionResult<ApiResponse<UserModel, object?>> getCurrentUser()
        {
            try
            {
                JwtDecoded decoded = HttpContext.Items["decoded"] as JwtDecoded;
                UserModel? currentUser = _userService.GetUserById(decoded.sub);

                return Ok(new ApiResponse<UserModel, object?>(currentUser)); 
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
