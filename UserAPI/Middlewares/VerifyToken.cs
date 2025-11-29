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

                var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    context.Result = new JsonResult(new
                    {
                        status_code = 401,
                        message = "Failed to authenticate because of bad credentials or an invalid authorization header."
                    })
                    {
                        StatusCode = 401
                    };
                    return;
                }

                string token = authHeader.Substring(7);

                JwtDecoded decoded = jwtToken.ValidateToken(token);

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