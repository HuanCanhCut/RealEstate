using System.Security.Cryptography;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using AdminAPI.Utils.Interfaces;

namespace AdminAPI.Utils
{
    public class JwtDecoded
    {
        public long exp { get; set; }
        public long iat { get; set; }
        public int sub { get; set; }
        public string jti { get; set; }
    }

    public class JWT : IJWT
    {
        public string GenerateToken(int userId, string secretKey, int expMinutes)
        {
            try
            {
                string token = JwtBuilder.Create()
                          .WithAlgorithm(new HMACSHA256Algorithm())
                          .WithSecret(secretKey)
                          .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(expMinutes).ToUnixTimeSeconds())
                          .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                          .AddClaim("sub", userId)
                          .AddClaim("jti", Guid.NewGuid().ToString())
                          .Encode();

                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JwtDecoded ValidateToken(string token, string secretKey)
        {
            try
            {

                JwtDecoded? json = JwtBuilder.Create()
                          .WithAlgorithm(new HMACSHA256Algorithm())
                          .WithSecret(secretKey)
                          .MustVerifySignature()
                          .Decode<JwtDecoded>(token);

                return json;
            }
            catch (TokenNotYetValidException)
            {
                throw new TokenNotYetValidException("Failed to authenticate because of bad credentials or an invalid authorization header.");
            }
            catch (TokenExpiredException)
            {
                throw new TokenExpiredException("Failed to authenticate because of expired token");
            }
            catch (SignatureVerificationException)
            {
                throw new TokenExpiredException("Failed to authenticate because of bad credentials or an invalid authorization header.");
            }
        }
    }
}
