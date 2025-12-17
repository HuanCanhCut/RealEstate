using JWT.Exceptions;
using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;
using UserAPI.Models.Enums;
using UserAPI.Repositories.Interfaces;
using UserAPI.Respositories.Interfaces;
using UserAPI.Services.Interfaces;
using UserAPI.Utils;
using UserAPI.Utils.Interfaces;
using static UserAPI.Errors.Error;

namespace UserAPI.Services
{
    public class AuthService(IUserRepository userRepository, IJWT jwt, ITokenRepository tokenRepository) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJWT _jwt = jwt;
        private readonly ITokenRepository _tokenRepository = tokenRepository;

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

                _tokenRepository.InsertRefreshToken(insertedUserId, refreshToken);

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

                _tokenRepository.InsertRefreshToken(userExist.id, refresh_token);

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

        public (string newAccessToken, string newRefreshToken) RefreshToken(string refresh_token)
        {
            try
            {
                try
                {
                    JwtDecoded decoded = _jwt.ValidateToken(refresh_token, Environment.GetEnvironmentVariable("JWT_REFRESH_SECRET_KEY")! ?? throw new Exception("JWT_REFRESH_SECRET_KEY is not set"));
                }
                catch (Exception ex)
                {
                    if (ex is TokenExpiredException)
                    {
                        _tokenRepository.RemoveRefreshToken(refresh_token);
                    }

                    throw;
                }

                BlacklistedTokenModel? blacklistedToken = _tokenRepository.GetBlacklistToken(refresh_token);

                RefreshTokenModel? refreshToken = _tokenRepository.GetRefreshToken(refresh_token);

                if (blacklistedToken != null || refreshToken == null)
                {
                    throw new UnauthorizedError("Token không hợp lệ hoặc đã hết hạn");
                }

                (string newAccessToken, string newRefreshToken) = GenerateTokens(refreshToken.user_id);

                int rowsAffected = _tokenRepository.UpdateRefreshToken(refreshToken.user_id, newRefreshToken, refresh_token);

                if (rowsAffected == 0)
                {
                    throw new BadRequestError("Không thể cập nhật token refresh");
                }

                return (newAccessToken, newRefreshToken);
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

        public void Logout(string refresh_token)
        {
            try
            {
                _tokenRepository.InsertBlacklistToken(refresh_token);
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
