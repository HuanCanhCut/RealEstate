using RealEstate.DTO.Response;
using RealEstate.Models;

namespace RealEstate.Services.Interfaces
{
    public interface IAuthService
    {
        ApiResponse<UserModel, MetaToken> Register(string email, string password);
    }
}
