using RealEstate.Models;

namespace RealEstate.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserModel? GetUserByEmail(string email);
        int CreateUser(UserModel user);
    }
}
