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
    }
}
