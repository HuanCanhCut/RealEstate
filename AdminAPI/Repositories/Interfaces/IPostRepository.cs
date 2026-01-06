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
    }
}
