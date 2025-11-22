using RealEstate.Models;
using RealEstate.Repositories.Interfaces;
using RealEstate.Services.Interfaces;
using static RealEstate.Errors.Error;

namespace RealEstate.Services
{
    public class AuthService(IUserRepository userRepository) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public UserModel Register(string email, string password)
        {
            try
            {
                UserModel? userExist = _userRepository.GetUserByEmail(email);

                if (userExist != null)
                {
                    throw new ConflictError("Email already exists");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                string fullName = email.Split('@')[0].ToString();

                UserModel newUser = new()
                {
                    full_name = fullName,
                    email = email,
                    password = hashedPassword,
                    nickname = fullName,
                    created_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                };

                int insertedUserId = _userRepository.CreateUser(newUser);

                return newUser;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message);
            }
        }
    }
}
