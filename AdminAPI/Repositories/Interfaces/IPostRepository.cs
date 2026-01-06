using AdminAPI.Models;
using AdminAPI.Models.Enums;

namespace AdminAPI.Repositories.Interfaces
{
    public interface IPostRepository
    {
        public PostModel GetPostById(int postId);
        public int UpdatePostStatus(int postId, PostEnum type);

        public int CountAll();

        public int DeletePost(int post_id);
        public List<PostModel> GetPosts(int page, int per_page, PostEnum? post_status, ProjectType? project_type, int? category_id);
    }
}
