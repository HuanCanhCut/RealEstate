using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;

using UserAPI.Middlewares;
using UserAPI.Models;
using UserAPI.Services.Interfaces;
using UserAPI.Utils;
using UserAPI.Utils.Interfaces;
using static UserAPI.Errors.Error;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController(IPostService postService, IJWT jwt) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly IJWT _jwt = jwt;

        [VerifyToken]
        [HttpPost]
        public ActionResult<ApiResponse<PostModel, object?>> CreatePost([FromBody] CreatePostRequest postRequest)
        {
            try
            {
                PostModel post = _postService.CreatePost(postRequest);

                return CreatedAtAction(nameof(CreatePost), post);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<PostModel>, MetaPagination>> GetPosts(
            [FromQuery] GetPostRequest postRequest
        )
        {
            try
            {
                int currentUserId = 0;

                try
                {
                    string accessToken = Request.Cookies["access_token"] ?? "";

                    if (string.IsNullOrEmpty(accessToken))
                    {
                        throw new Exception();
                    }

                    JwtDecoded decoded = _jwt.ValidateToken(accessToken, Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "");
                    currentUserId = decoded.sub;
                }
                catch (Exception)
                {
                    currentUserId = 0;
                }

                GetPostServiceResponse posts = _postService.GetPosts(postRequest, currentUserId);

                return Ok(new ApiResponse<List<PostModel>, MetaPagination>(
                    data: posts.data.ToList(),
                    meta: new MetaPagination(
                        total: posts.total,
                        count: posts.count,
                        current_page: postRequest.page,
                        per_page: postRequest.per_page
                    )
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("search")]
        public ActionResult<ApiResponse<List<PostModel>, MetaPagination>> SearchPosts(
               [FromQuery] string q
           )
        {
            try
            {
                if (String.IsNullOrEmpty(q))
                {
                    throw new BadRequestError("q is required");
                }

                List<PostModel> posts = _postService.SearchPosts(q);

                return Ok(new ApiResponse<List<PostModel>, object?>(
                    data: posts
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [VerifyToken]
        [HttpPost("{id}/like")]
        public ActionResult<ApiResponse<PostModel, object?>> LikePost([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new BadRequestError("post_id phải lớn hơn 0");
                }

                JwtDecoded decoded = HttpContext.Items["decoded"] as JwtDecoded;

                PostModel post = _postService.LikePost(post_id: id, user_id: decoded.sub);

                return Ok(new ApiResponse<PostModel, object?>(post));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [VerifyToken]
        [HttpDelete("{id}/unlike")]
        public ActionResult<ApiResponse<PostModel, object?>> UnlikePost([FromRoute] int id)
        {
            try
            {

                if (id <= 0)
                {
                    throw new BadRequestError("post_id phải lớn hơn 0");
                }

                JwtDecoded decoded = HttpContext.Items["decoded"] as JwtDecoded;

                _postService.UnlikePost(post_id: id, user_id: decoded.sub);

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [VerifyToken]
        [HttpPut("{id}/update")]
        public ActionResult<ApiResponse<PostModel, object?>> UpdatePost([FromRoute] int id, [FromBody] UpdatePostRequest request)
        {
            try
            {
                PostModel post = _postService.UpdatePost(id, request);

                return Ok(new ApiResponse<PostModel, object?>(post));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [VerifyToken]
        [HttpDelete("{post_id}")]
        public IActionResult DeletePost([FromRoute] int post_id)
        {
            try
            {
                JwtDecoded decoded = HttpContext.Items["decoded"] as JwtDecoded;

                if (post_id <= 0)
                {
                    throw new BadRequestError("post_id phải lớn hơn 0");
                }

                _postService.DeletePost(post_id, decoded.sub);

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<PostModel, object?>> GetPostById([FromRoute] int id)
        {
            try
            {
                PostModel? post = _postService.GetPostById(id);
                if (post == null)
                {
                    throw new NotFoundError("Không tìm thấy bài đăng.");
                }
                return Ok(new ApiResponse<PostModel, object?>(post, null));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}