using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.Middlewares;
using UserAPI.Models;
using UserAPI.Services.Interfaces;
using UserAPI.Utils;

namespace UserAPI.Controllers
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

        [HttpGet("{nickname}")]
        public ActionResult<ApiResponse<UserModel, object?>> getUserByNickname([FromRoute] string nickname)
        {
            try
            {
                UserModel? user = _userService.GetUserByNickname(nickname);
                return Ok(new ApiResponse<UserModel, object?>(user));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("me/update")]
        public ActionResult<ApiResponse<UserModel, object?>> updateCurrentUser([FromBody] UpdateCurrentUserRequest request)
        {
            try
            {
                JwtDecoded decoded = HttpContext.Items["decoded"] as JwtDecoded;

                UserModel? updatedUser = _userService.UpdateCurrentUser(decoded.sub, request);

                return Ok(new ApiResponse<UserModel, object?>(updatedUser));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet()]
        public ActionResult<ApiResponse<List<UserModel>, MetaPagination>> getAllUsers([FromQuery] PaginationRequest query)
        {
            try
            {
                ServiceResposePagination<UserModel> users = _userService.GetAllUsers(query.page, query.per_page);

                return Ok(new ApiResponse<List<UserModel>, MetaPagination>(
                    data: users.data ?? new List<UserModel>(),
                    meta: new MetaPagination(
                        total: users.total,
                        count: users.count,
                        current_page: query.page,
                        per_page: query.per_page
                    )
                ));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
