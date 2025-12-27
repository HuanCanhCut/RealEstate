using AdminAPI.Models;

namespace AdminAPI.Repositories.Interfaces
{
    public interface IPostRepository
    {
        public PostModel GetPostById(int postId);
        public int ApprovePost(int postId);
        public int CountAll();
    }
}
