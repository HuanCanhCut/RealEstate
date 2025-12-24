using AdminAPI.Models;

namespace AdminAPI.Services.Interfaces
{
    public interface IPostService
    {
        public int ApprovePost(int postId);
        public PostModel GetPostById(int postId);
    }
}
