using RealEstate.DTO.Response;
using RealEstate.Models;
using RealEstate.Repositories.Interfaces;
using RealEstate.Services.Interfaces;
using RealEstate.Utils.Interfaces;
using static RealEstate.Errors.Error;

namespace RealEstate.Services
{
    public class AuthService(IUserRepository userRepository, IJWT jwt) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJWT _jwt = jwt;

        public ApiResponse<UserModel, MetaToken> Register(string email, string password)
        {
            try
            {
                UserModel? userExist = _userRepository.GetUserByEmail(email);

                if (userExist != null)
                {
                    throw new ConflictError("Email already exists");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                string emailName = email.Split('@')[0].ToString();

                UserModel newUser = new()
                {
                    full_name = emailName,
                    email = email,
                    password = hashedPassword,
                    nickname = Guid.NewGuid().ToString(),
                    created_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                };

                int insertedUserId = _userRepository.CreateUser(newUser);

                string token = _jwt.GenerateToken(insertedUserId);

                return new ApiResponse<UserModel, MetaToken>(newUser, new MetaToken { access_token = token });
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
