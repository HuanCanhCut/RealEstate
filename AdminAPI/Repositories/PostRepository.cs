using AdminAPI.Models;
using AdminAPI.Models.Enums;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;

namespace AdminAPI.Repositories
{
    public class PostRepository : IPostRepository
    {
        private DbContext _dbContext;

        public PostRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int UpdatePostStatus(int postId, PostEnum type)
        {
            try
            {
                string sql = $"UPDATE posts SET post_status = '{type}' WHERE id = {postId}";
                return _dbContext.ExecuteNonQuery(sql);
            }
            catch (Exception)
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
            catch (Exception) { throw; }
        }

        public int CountAll()
        {
            try
            {
                string sql = $"SELECT COUNT(id) FROM posts";

                return Convert.ToInt32(_dbContext.ExecuteScalar(sql) ?? 0);
            }
            catch (Exception) { throw; }
        }

        public int DeletePost(int post_id)
        {
            try
            {
                string sql = $"UPDATE posts SET is_deleted = 1, deleted_at = CURRENT_TIME WHERE id = {post_id}";
                return _dbContext.ExecuteNonQuery(sql);
            }
            catch (Exception) { throw; }
        }
    }
}
