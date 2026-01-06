using JWT.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserAPI.Models;
using UserAPI.Repositories;
using UserAPI.Utils;
using UserAPI.Utils.Interfaces;

namespace UserAPI.Middlewares
{
    [AttributeUsage(AttributeTargets.All)]
    public class VerifyTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var jwtToken = context.HttpContext.RequestServices.GetService<IJWT>();

                if (jwtToken == null)
                {
                    context.Result = new JsonResult(new
                    {
                        status_code = 500,
                        message = "JWT service not configured"
                    })
                    {
                        StatusCode = 500
                    };
                    return;
                }

                string? token = context.HttpContext.Request.Cookies["access_token"];

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Failed to authenticate because no token provided.");
                }

                // check token in blacklist
                TokenRepository? tokenRepository = context.HttpContext.RequestServices.GetService<TokenRepository>();

                BlacklistedTokenModel? blacklistedToken = tokenRepository?.GetBlacklistToken(token);

                if (blacklistedToken != null)
                {
                    throw new Exception("Failed to authenticate because of bad credentials or an invalid authorization header.");
                }

                JwtDecoded decoded = jwtToken.ValidateToken(token, Environment.GetEnvironmentVariable("JWT_SECRET_KEY")! ?? throw new Exception("JWT_SECRET_KEY is not set"));

                if (string.IsNullOrEmpty(decoded.sub.ToString()))
                {
                    throw new Exception("Failed to authenticate because of expired token.");
                }

                context.HttpContext.Items["decoded"] = decoded;

                await next();
            }
            catch (Exception ex)
            {
                // if token is expired, set header x-refresh-token-required
                if (ex is TokenExpiredException)
                {
                    context.HttpContext.Response.Headers.Append("x-refresh-token-required", "true");
                }
                else
                {
                    // clear cookies from client (only delete token when error is not token expired)
                    context.HttpContext.Response.Cookies.Delete("access_token", new CookieOptions { SameSite = SameSiteMode.None, Secure = true });
                    context.HttpContext.Response.Cookies.Delete("refresh_token", new CookieOptions { SameSite = SameSiteMode.None, Secure = true });
                }

                context.Result = new JsonResult(new
                {
                    status_code = 401,
                    message = ex.Message
                })
                {
                    StatusCode = 401
                };
            }
        }
    }
}