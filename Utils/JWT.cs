using System.Security.Cryptography;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using RealEstate.Utils.Interfaces;

namespace RealEstate.Utils
{
    public class JwtPayload
    {
        public long exp { get; set; }
        public long iat { get; set; }
        public int sub { get; set; }
        public string jti { get; set; }
    }

    public class JWT : IJWT
    {
        public string GenerateToken(int userId)
        {
            try
            {
                string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")! ?? throw new Exception("JWT_SECRET_KEY is not set");

                string token = JwtBuilder.Create()
                          .WithAlgorithm(new HMACSHA256Algorithm())
                          .WithSecret(secretKey)
                          .AddClaim("exp", DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds())
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

        public JwtPayload ValidateToken(string token)
        {
            try
            {
                string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")! ?? throw new Exception("JWT_SECRET_KEY is not set");

                var json = JwtBuilder.Create()
                          .WithAlgorithm(new HMACSHA256Algorithm())
                          .WithSecret(secretKey)
                          .MustVerifySignature()
                          .Decode<JwtPayload>(token);

                return json;
            }
            catch (TokenNotYetValidException)
            {
                throw new Exception("Failed to authenticate because of bad credentials or an invalid authorization header.");
            }
            catch (TokenExpiredException)
            {
                throw new Exception("Failed to authenticate because of expired token");
            }
            catch (SignatureVerificationException)
            {
                throw new Exception("Failed to authenticate because of bad credentials or an invalid authorization header.");
            }
        }
    }
}
