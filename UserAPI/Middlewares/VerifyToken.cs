using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

                string token = context.HttpContext.Request.Cookies["access_token"];

                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new JsonResult(new
                    {
                        status_code = 401,
                        message = "Failed to authenticate because no token provided."
                    })
                    {
                        StatusCode = 401
                    };
                    return;
                }

                JwtDecoded decoded = jwtToken.ValidateToken(token, Environment.GetEnvironmentVariable("JWT_SECRET_KEY")! ?? throw new Exception("JWT_SECRET_KEY is not set"));

                if (string.IsNullOrEmpty(decoded.sub.ToString()))
                {
                    context.Result = new JsonResult(new
                    {
                        status_code = 401,
                        message = "Failed to authenticate because of expired token."
                    })
                    {
                        StatusCode = 401
                    };
                    return;
                }

                context.HttpContext.Items["decoded"] = decoded;

                await next();
            }
            catch (Exception ex)
            {

                // clear cookies from client
                context.HttpContext.Response.Cookies.Delete("access_token");
                context.HttpContext.Response.Cookies.Delete("refresh_token");

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