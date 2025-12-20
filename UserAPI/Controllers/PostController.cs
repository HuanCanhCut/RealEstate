using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [VerifyToken]
    [ApiController]
    [Route("api/posts")]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

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
                GetPostServiceResponse posts = _postService.GetPosts(postRequest);

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