using AdminAPI.DTO.Request;
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

                public ServiceResponsePagination<PostModel> GetPosts(GetPostsRequest request);
        }
}
