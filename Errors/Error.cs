namespace RealEstate.Errors
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
            public BadRequestError(string message = "Bad Request")
                : base(message, StatusCodes.Status400BadRequest)
            {
            }
        }

        // 401: Unauthorized
        public class UnauthorizedError : AppError
        {
            public UnauthorizedError(string message = "Unauthorized")
                : base(message, StatusCodes.Status401Unauthorized)
            {
            }
        }

        // 403: Forbidden
        public class ForbiddenError : AppError
        {
            public ForbiddenError(string message = "Forbidden")
                : base(message, StatusCodes.Status403Forbidden)
            {
            }
        }

        // 404: Not Found
        public class NotFoundError : AppError
        {
            public NotFoundError(string message = "Not Found")
                : base(message, StatusCodes.Status404NotFound)
            {
            }
        }

        // 409: Conflict
        public class ConflictError : AppError
        {
            public ConflictError(string message = "Conflict")
                : base(message, StatusCodes.Status409Conflict)
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
            public TooManyRequestsError(string message = "Too Many Requests")
                : base(message, StatusCodes.Status429TooManyRequests)
            {
            }
        }

        // 500: Internal Server Error
        public class InternalServerError : AppError
        {
            public InternalServerError(string message = "Internal Server Error")
                : base(message, StatusCodes.Status500InternalServerError)
            {
            }
        }

        // 501: Not Implemented
        public class NotImplementedError : AppError
        {
            public NotImplementedError(string message = "Not Implemented")
                : base(message, StatusCodes.Status501NotImplemented)
            {
            }
        }
    }
}