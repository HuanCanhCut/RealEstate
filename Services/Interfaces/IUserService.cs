using RealEstate.Models;

namespace RealEstate.Services.Interfaces
{
    public interface IUserService
    {
        UserModel GetUserById(int id);
        UserModel GetUserByNickname(string nickname);
    }
}
