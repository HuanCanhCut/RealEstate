using RealEstate.DTO.Response;
using RealEstate.DTO.ServiceResponse;
using RealEstate.Models;

namespace RealEstate.Services.Interfaces
{
    public interface IAuthService
    {
        RegisterServiceResponse Register(string email, string password);
        LoginServiceResponse Login(string email, string password);
    }
}
