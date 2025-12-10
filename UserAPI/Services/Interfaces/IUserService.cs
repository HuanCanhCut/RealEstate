using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.Models;

namespace UserAPI.Services.Interfaces
{
    public interface IUserService
    {
        UserModel GetUserById(int id);
        UserModel GetUserByNickname(string nickname);
        UserModel? UpdateCurrentUser(int id, UpdateCurrentUserRequest request);
        ServiceResposePagination<UserModel> GetAllUsers(int page, int per_page);
    }
}
