using AdminAPI.Models;
using AdminAPI.Models.Enums;
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

        public int UpdatePostStatus(int postId, PostEnum type)
        {
            try
            {
                string sql = $"UPDATE posts SET post_status = '{type}' WHERE id = {postId}";
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
