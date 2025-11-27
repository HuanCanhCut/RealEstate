using RealEstate.Models;

namespace RealEstate.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserModel? GetUserByEmail(string email);
        int CreateUser(UserModel user);
        int UpdateUserPassword(int id, string password);

        UserModel? GetUserById(int id);
        UserModel? GetUserByNickname(string nickname);
    }
}
