using UserAPI.DTO.Request;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserRepository(DbContext dbContext) : IUserRepository
    {
        private readonly DbContext _dbContext = dbContext;

        public UserModel? GetUserByEmail(string email)
        {
            try
            {
                string query = $"SELECT * FROM users WHERE email = '{email}'";

                return _dbContext.ExecuteQuery(query).ConvertTo<UserModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateUser(UserModel user)
        {
            try
            {
                string query = @$"
                INSERT INTO 
                    users (full_name, email, password, nickname, created_at, updated_at) 
                VALUES ('{user.full_name}', '{user.email}', '{user.password}', '{user.nickname}', NOW(), NOW());

                SELECT LAST_INSERT_ID();
            ";

                return Convert.ToInt32(_dbContext.ExecuteScalar(query));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateUserPassword(int id, string password)
        {
            try
            {
                string query = $"UPDATE users SET password = '{password}' WHERE id = {id}";

                return _dbContext.ExecuteNonQuery(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserModel GetUserById(int id)
        {
            try
            {
                string query = $"SELECT * FROM users WHERE id = {id}";
                return _dbContext.ExecuteQuery(query).ConvertTo<UserModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public UserModel GetUserByNickname(string nickname)
        {
            try
            {
                string query = @$"SELECT * FROM users WHERE MATCH (nickname) AGAINST ('{nickname}')";
                return _dbContext.ExecuteQuery(query).ConvertTo<UserModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateCurrentUser(int currentUserId, UpdateCurrentUserRequest request)
        {
            try
            {
                string sql = $@"
                    UPDATE users
                    SET 
                        full_name = '{request.full_name}', 
                        nickname = '{request.nickname}', 
                        avatar = '{request.avatar_url}',
                        phone_number = '{request.phone_number}',
                        updated_at = NOW()
                    WHERE id = {currentUserId}
                ";

                return _dbContext.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> GetAllUsers(int page, int per_page)
        {
            try
            {
                int offset = (page - 1) * per_page;
                string query = $@"
                    SELECT 
                      users.*,
                      COUNT(posts.id) as post_count
                    FROM users
                    LEFT JOIN posts ON posts.user_id = users.id
                    GROUP BY users.id
                    LIMIT {per_page} OFFSET {offset}";
                return _dbContext.ExecuteQuery(query).ConvertTo<UserModel>() ?? new List<UserModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CountAll()
        {
            try
            {
                string query = "SELECT COUNT(1) FROM users";
                return Convert.ToInt32(_dbContext.ExecuteScalar(query));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
