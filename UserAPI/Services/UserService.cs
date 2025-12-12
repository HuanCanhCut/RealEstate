using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;
using static UserAPI.Errors.Error;

namespace UserAPI.Services
{
    public class UserService : IUserService
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

                if (user == null)
                {
                    throw new UnauthorizedError("Unauthorize");
                }
                return user;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public UserModel? GetUserByNickname(string nickname)
        {
            try
            {
                UserModel? user = _userRepository.GetUserByNickname(nickname);

                if (user == null)
                {
                    throw new UnauthorizedError("Không tìm thấy người dùng này!");
                }
                return user;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public UserModel? UpdateCurrentUser(int currentUserId, UpdateCurrentUserRequest request)
        {
            try
            {
                int rowsAffected = _userRepository.UpdateCurrentUser(currentUserId, request);

                if (rowsAffected == 0)
                {
                    throw new UnauthorizedError("Unauthorize");
                }

                UserModel? updatedUser = _userRepository.GetUserById(currentUserId);

                return updatedUser;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }
                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public ServiceResponsePagination<UserModel> GetAllUsers(int page, int per_page)
        {
            try
            {
                List<UserModel> users = _userRepository.GetAllUsers(page, per_page);
                int total = _userRepository.CountAll();

                return new ServiceResponsePagination<UserModel>
                {
                    count = users.Count(),
                    total = total,
                    data = users,
                };
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }
                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

    }
}
