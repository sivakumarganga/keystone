using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KeyStone.Shared.API
{
  
    public class ApiResponse
    {
        public ApiResponse(bool isSuccess, ApiError error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public ApiError Error { get; }

        public static ApiResponse Success() => new(true, ApiError.None);

        public static ApiResponse Failure(ApiError error) => new(false, error);

        public static ApiResponse<T> Success<T>(T data) => new(true, ApiError.None, data);

        public static ApiResponse<T> Failure<T>(ApiError error) => new(false, error, default);
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; }

        public ApiResponse(bool isSuccess, ApiError error, T? data) : base(isSuccess, error)
        {
            Data = data;
        }
    }
    public class ApiError
    {
        public ApiError(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public static ApiError None => new(string.Empty);

        public static implicit operator ApiError(string message) => new(message);

        public static implicit operator string(ApiError error) => error.Message;
    }
}
