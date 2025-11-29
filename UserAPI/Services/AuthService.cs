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

                string token = _jwt.GenerateToken(insertedUserId);

                return new RegisterServiceResponse()
                {
                    user = newUser,
                    meta = new MetaToken { access_token = token }
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

                string token = _jwt.GenerateToken(userExist.id);

                return new LoginServiceResponse()
                {
                    user = userExist,
                    meta = new MetaToken { access_token = token }
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
