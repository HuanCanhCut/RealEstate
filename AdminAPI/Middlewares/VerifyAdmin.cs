using JWT.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AdminAPI.Models;
using AdminAPI.Utils;
using AdminAPI.Utils.Interfaces;
using AdminAPI.Repositories;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Models.Enums;

namespace AdminAPI.Middlewares
{
    [AttributeUsage(AttributeTargets.All)]
    public class VerifyAdminAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                JwtDecoded decoded = context.HttpContext.Items["decoded"] as JwtDecoded;

                if (decoded == null)
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

                UserRepository userRepository = context.HttpContext.RequestServices.GetRequiredService<UserRepository>();

                UserModel? user = userRepository.GetUserById(decoded.sub);

                if (user == null)
                {
                    context.Result = new JsonResult(new
                    {
                        status_code = 404,
                        message = "Failed to determine admin because user not found."
                    })
                    {
                        StatusCode = 404
                    };
                    return;
                }

                if (user.role != UserRole.admin)
                {
                    context.Result = new JsonResult(new
                    {
                        status_code = 403,
                        message = "Failed to determine admin because user is not an admin."
                    })
                    {
                        StatusCode = 403
                    };
                    return;
                }

                await next();
            }
            catch (Exception ex)
            {
                context.Result = new JsonResult(new
                {
                    status_code = 500,
                    message = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "production" ? "Internal server error." : ex.Message
                })
                {
                    StatusCode = 500
                };
                return;
            }
        }
    }
}