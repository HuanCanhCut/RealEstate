using UserAPI.DTO.Request;
using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserModel? GetUserByEmail(string email);
        int CreateUser(UserModel user);
        int UpdateUserPassword(int id, string password);

        UserModel? GetUserById(int id);
        UserModel? GetUserByNickname(string nickname);

        int UpdateCurrentUser(int currentUserID, UpdateCurrentUserRequest request);
        List<UserModel> GetAllUsers(int page, int per_page);
        int CountAll();
    }
}
