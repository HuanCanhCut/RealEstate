using AdminAPI.DTO.Request;
using AdminAPI.DTO.Response;
using AdminAPI.Middlewares;
using AdminAPI.Models;
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
        public PostManagerController(IPostService postService)
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

        [HttpDelete("{id}/admin")]
        public ActionResult DeletePost(int id)
        {
            try
            {
                _postService.DeletePost(id);
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<PostModel>, MetaPagination>> GetPosts([FromQuery] GetPostsRequest request)
        {
            try
            {
                ServiceResponsePagination<PostModel> posts = _postService.GetPosts(request.page, request.per_page, request.post_status, request.project_type, request.category_id);
                return Ok(new ApiResponse<List<PostModel>, MetaPagination>(
                    data: posts.data,
                    meta: new MetaPagination(
                        total: posts.total,
                        count: posts.count,
                        current_page: request.page,
                        per_page: request.per_page
                    )
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
