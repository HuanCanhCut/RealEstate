using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;

namespace UserAPI.Services.Interfaces
{
    public interface IAuthService
    {
        RegisterServiceResponse Register(string email, string password);
        LoginServiceResponse Login(string email, string password);
        void ChangePassword(string email, string password, string new_password);
    }
}
