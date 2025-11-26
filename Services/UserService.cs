using RealEstate.Models;
using RealEstate.Repositories.Interfaces;
using RealEstate.Services.Interfaces;
using static RealEstate.Errors.Error;

namespace RealEstate.Services
{
    public class UserService: IUserService
    {

        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            
        }

        public UserModel GetUserById(int id)
        {
            try
            {
                UserModel? user = _userRepository.GetUserById(id);

                if( user == null)
                {
                    throw new UnauthorizedError("Unauthorize");
                }
                return user;
            }
            catch (Exception ex)
            {
                if(ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }
    }
}
