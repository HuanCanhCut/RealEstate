using static RealEstate.Errors.Error;

namespace RealEstate.Middlewares
{
    public class GlobalError(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is AppError appError)
            {
                string message = appError.Message;

                if (appError.StatusCode == StatusCodes.Status500InternalServerError && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                {
                    message = "Internal server error";
                }

                var response = new
                {
                    status_code = appError.StatusCode,
                    message,
                    error = appError.Error
                };

                context.Response.StatusCode = appError.StatusCode;
                return context.Response.WriteAsJsonAsync(response);
            }

            context.Response.StatusCode = 500;
            return context.Response.WriteAsJsonAsync(new
            {
                status_code = 500,
                message = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ? "Internal server error" : exception.Message
            });
        }
    }
}
