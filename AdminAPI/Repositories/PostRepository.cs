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

        public List<PostModel> GetPosts(int page, int per_page, PostEnum? post_status, ProjectType? project_type, int? category_id)
        {
            try
            {
                string sql = @$"
                    SELECT *, 
                        JSON_OBJECT(
                            'id', post_details.id,
                            'price', post_details.price,
                            'created_at', post_details.created_at,
                            'updated_at', post_details.updated_at
                        ) as json_post_detail
                    FROM posts
                    JOIN post_details ON posts.id = post_details.post_id
                    WHERE is_deleted = 0
                    AND deleted_at IS NULL
                    AND 1=1
                ";

                if (post_status != null)
                {
                    sql += $" AND post_status = '{post_status}'";
                }
                if (project_type != null)
                {
                    sql += $" AND project_type = '{project_type}'";
                }
                if (category_id != null)
                {
                    sql += $" AND category_id = {category_id}";
                }

                sql += $" ORDER BY posts.id DESC";
                sql += $" LIMIT {per_page} OFFSET {(page - 1) * per_page}";

                return _dbContext.ExecuteQuery(sql).ConvertTo<PostModel>()!;
            }
            catch (Exception) { throw; }
        }
    }
}
