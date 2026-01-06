using AdminAPI.DTO.Response;
using AdminAPI.Models;
using AdminAPI.Models.Enums;

namespace AdminAPI.Services.Interfaces
{
        public interface IPostService
        {
                public int UpdatePostStatus(int postId, PostEnum status);
                public PostModel GetPostById(int postId);

                public int CountAll();

                public void DeletePost(int post_id);

                public ServiceResponsePagination<PostModel> GetPosts(int page, int per_page, PostEnum? post_status, ProjectType? project_type, int? category_id);
        }
}
