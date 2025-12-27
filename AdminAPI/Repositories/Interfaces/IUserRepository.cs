using AdminAPI.DTO.Request;
using AdminAPI.Models;

namespace AdminAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserModel? GetUserById(int id);
        bool DeleteUser(int id);
    }
}
