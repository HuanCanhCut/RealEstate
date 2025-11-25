using RealEstate.Models;
using RealEstate.Repositories.Interfaces;

namespace RealEstate.Repositories
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

                return _dbContext.ExecuteNonQuery(query);
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
    }
}
