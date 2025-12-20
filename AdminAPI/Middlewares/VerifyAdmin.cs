using JWT.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AdminAPI.Models;
using AdminAPI.Utils;
using AdminAPI.Utils.Interfaces;
using AdminAPI.Repositories;

namespace AdminAPI.Middlewares
{
    [AttributeUsage(AttributeTargets.All)]
    public class VerifyAdminAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {

            }
            catch (Exception ex)
            {

                // 
            }
        }
    }
}