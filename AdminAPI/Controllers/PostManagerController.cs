using AdminAPI.DTO.Response;
using AdminAPI.Middlewares;
using AdminAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminAPI.Controllers
{
    [VerifyToken]
    [VerifyAdmin]
    [Route("api/manager/posts")]
    [ApiController]
    public class PostManagerController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostManagerController(IPostService postService )
        {
            _postService = postService;
        }

        [HttpPatch("{id}/approve")]
        public ActionResult ApprovePost(int id)
        {
            try
            {
                _postService.ApprovePost(id);
                return Ok(new
                {
                    message = $"Post id {id} đã được duyệt thành công.",
                    status = 200
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
