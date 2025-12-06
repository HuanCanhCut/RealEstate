using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;
using UserAPI.Models.Enums;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;
using UserAPI.Utils.Interfaces;
using static UserAPI.Errors.Error;

namespace UserAPI.Services
{
    public class AuthService(IUserRepository userRepository, IJWT jwt) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJWT _jwt = jwt;

        private (string access_token, string refresh_token) GenerateTokens(int userId)
        {
            try
            {
                string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")! ?? throw new Exception("JWT_SECRET_KEY is not set");
                string refreshSecretKey = Environment.GetEnvironmentVariable("JWT_REFRESH_SECRET_KEY")! ?? throw new Exception("JWT_REFRESH_SECRET_KEY is not set");

                string accessToken = _jwt.GenerateToken(userId, secretKey, 30); // 30 minutes
                string refreshToken = _jwt.GenerateToken(userId, refreshSecretKey, 60 * 24 * 30 * 6); // 6 months

                return (accessToken, refreshToken);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public RegisterServiceResponse Register(string email, string password)
        {
            try
            {
                UserModel? userExist = _userRepository.GetUserByEmail(email);

                if (userExist != null)
                {
                    throw new BadRequestError("Không thể đăng kí tài khoản, vui lòng thử lại");
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
                    updated_at = DateTime.UtcNow,
                    role = UserRole.user
                };

                int insertedUserId = _userRepository.CreateUser(newUser);


                (string accessToken, string refreshToken) = GenerateTokens(insertedUserId);

                return new RegisterServiceResponse()
                {
                    user = newUser,
                    access_token = accessToken,
                    refresh_token = refreshToken
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

        public LoginServiceResponse Login(string email, string password)
        {
            try
            {
                UserModel? userExist = _userRepository.GetUserByEmail(email);

                if (userExist == null)
                {
                    throw new UnauthorizedError("Tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại");
                }

                if (!BCrypt.Net.BCrypt.Verify(password, userExist.password))
                {
                    throw new UnauthorizedError("Tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại");
                }

                (string access_token, string refresh_token) = GenerateTokens(userExist.id);

                return new LoginServiceResponse()
                {
                    user = userExist,
                    access_token = access_token,
                    refresh_token = refresh_token
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

        public void ChangePassword(string email, string password, string new_password)
        {
            try
            {
                LoginServiceResponse userValid = this.Login(email, password);

                UserModel user = userValid.user;

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(new_password);

                _userRepository.UpdateUserPassword(user.id, hashedPassword);
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
