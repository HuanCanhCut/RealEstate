using UserAPI.Models;

namespace UserAPI.Services.Interfaces
{
    public interface IUserService
    {
        UserModel GetUserById(int id);
        UserModel GetUserByNickname(string nickname);
    }
}
