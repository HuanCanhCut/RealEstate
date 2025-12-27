namespace AdminAPI.Errors
{
    public class Error
    {
        public class AppError : Exception
        {
            public int StatusCode { get; }
            public object? Error { get; }

            public AppError(string message, int statusCode, object? error = null)
                : base(message)
            {
                StatusCode = statusCode;
                Error = error;
            }
        }

        // 400: Bad Request
        public class BadRequestError : AppError
        {
            public BadRequestError(string message = "Bad Request", object? error = null)
                : base(message, StatusCodes.Status400BadRequest, error)
            {
            }
        }

        // 401: Unauthorized
        public class UnauthorizedError : AppError
        {
            public UnauthorizedError(string message = "Unauthorized", object? error = null)
                : base(message, StatusCodes.Status401Unauthorized, error)
            {
            }
        }

        // 403: Forbidden
        public class ForbiddenError : AppError
        {
            public ForbiddenError(string message = "Forbidden", object? error = null)
                : base(message, StatusCodes.Status403Forbidden, error)
            {
            }
        }

        // 404: Not Found
        public class NotFoundError : AppError
        {
            public NotFoundError(string message = "Not Found", object? error = null)
                : base(message, StatusCodes.Status404NotFound, error)
            {
            }
        }

        // 409: Conflict
        public class ConflictError : AppError
        {
            public ConflictError(string message = "Conflict", object? error = null)
                : base(message, StatusCodes.Status409Conflict, error)
            {
            }
        }

        // 422: Unprocessable Entity
        public class UnprocessableEntityError : AppError
        {
            public UnprocessableEntityError(string message = "Unprocessable Entity", object? error = null)
                : base(message, StatusCodes.Status422UnprocessableEntity, error)
            {
            }
        }

        // 429: Too Many Requests
        public class TooManyRequestsError : AppError
        {
            public TooManyRequestsError(string message = "Too Many Requests", object? error = null)
                : base(message, StatusCodes.Status429TooManyRequests, error)
            {
            }
        }

        // 500: Internal Server Error
        public class InternalServerError : AppError
        {
            public InternalServerError(string message = "Internal Server Error", object? error = null)
                : base(message, StatusCodes.Status500InternalServerError, error)
            {
            }
        }

        // 501: Not Implemented
        public class NotImplementedError : AppError
        {
            public NotImplementedError(string message = "Not Implemented", object? error = null)
                : base(message, StatusCodes.Status501NotImplemented, error)
            {
            }
        }
    }
}