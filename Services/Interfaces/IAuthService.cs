using RealEstate.Models;

namespace RealEstate.Services.Interfaces
{
    public interface IAuthService
    {
        UserModel Register(string email, string password);
    }
}
