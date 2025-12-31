using AdminAPI.DTO.Response;
using AdminAPI.Middlewares;
using AdminAPI.Models.Enums;
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
                _postService.UpdatePostStatus(id, PostEnum.approved);
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

        [HttpPatch("{id}/pending")]
        public ActionResult UpdatePostPending(int id)
        {
            try
            {
                _postService.UpdatePostStatus(id, PostEnum.pending);
                return Ok(new
                {
                    message = $"Post id {id} đã được cập nhật thành đang chờ duyệt.",
                    status = 200
                });
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPatch("{id}/reject")]
        public ActionResult UpdateRejectPost(int id)
        {
            try
            {
                _postService.UpdatePostStatus(id, PostEnum.rejected);
                return Ok(new
                {
                    message = $"Post id {id} đã được chuyển sang trạng thái từ chối.",
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
