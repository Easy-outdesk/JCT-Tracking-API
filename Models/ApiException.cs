namespace JCT_Tracking_Api.Models
{
    public class ApiException
    {
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }


        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }

            public static ApiResponse<T> SuccessResponse(T data, string message)
            {
                return new ApiResponse<T>
                {
                    Success = true,
                    Message = message,
                    Data = data
                };
            }

            public static ApiResponse<T> FailResponse(string message)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = message,
                    Data = default
                };
            }
        
    }
}
