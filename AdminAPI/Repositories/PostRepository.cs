using AdminAPI.Models;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;

namespace AdminAPI.Repositories
{
    public class PostRepository: IPostRepository
    {
        private DbContext _dbContext;

        public PostRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int ApprovePost(int postId)
        {
            try
            {
                string sql = $"UPDATE posts SET post_status = 'approved' WHERE id = {postId}";
                return _dbContext.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw;
            }
            }

        public PostModel GetPostById(int postId)
        {
            try
            {
                string sql = $"SELECT * FROM posts WHERE id = {postId}";

                return _dbContext.ExecuteQuery(sql).ConvertTo<PostModel>()?.FirstOrDefault()!;
            }
            catch(Exception) { throw;  }
        }
    }
}
