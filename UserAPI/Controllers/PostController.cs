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
using static UserAPI.Errors.Error;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

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
    }
}