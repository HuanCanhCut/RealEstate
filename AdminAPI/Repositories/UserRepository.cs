using AdminAPI.DTO.Request;
using AdminAPI.Models;
using AdminAPI.Repositories.Interfaces;

namespace AdminAPI.Repositories
{
    public class UserRepository(DbContext dbContext) : IUserRepository
    {
        private readonly DbContext _dbContext = dbContext;

        public UserModel? GetUserById(int id)
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

        public bool DeleteUser(int id)
        {
            try
            {
                string query = $"UPDATE users SET is_deleted = TRUE WHERE id = {id}";

                return _dbContext.ExecuteNonQuery(query) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
