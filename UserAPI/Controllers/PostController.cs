using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.Middlewares;
using UserAPI.Models;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    [VerifyToken]
    [ApiController]
    [Route("api/posts")]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [HttpPost]
        public ActionResult<ApiResponse<PostModel, object?>> CreatePost([FromBody] PostRequest postRequest)
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
    }
}