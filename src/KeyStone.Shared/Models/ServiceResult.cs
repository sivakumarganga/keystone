namespace KeyStone.Shared.Models;

public class ServiceResult<TResult>
{
    public TResult Result { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string? MessageCode { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsNotFoundError { get; set; }
    public bool IsFailedWithCode { get; set; }
    
    public ServiceResult()
    {
        
    }

    public ServiceResult(TResult result)
    {
        Result = result;
    }

    public static implicit operator ServiceResult<TResult>(TResult result)
    {
        return SuccessResult(result);
    }

    public static ServiceResult<TResult> SuccessResult(TResult result)
    {
        return new ServiceResult<TResult>()
        {
            IsSuccess = true,
            Result = result,
            ErrorMessage = string.Empty,
            Message = string.Empty,
            IsNotFoundError = false,
            IsFailedWithCode = false
        };
    }
    public static ServiceResult<TResult> SuccessResult(TResult result, string message)
    {
        return new ServiceResult<TResult>()
        {
            IsSuccess = true,
            Result = result,
            ErrorMessage = string.Empty,
            Message = message,
            IsNotFoundError = false,
            IsFailedWithCode = false
        };
    }
    
    public static ServiceResult<TResult> FailureResult(string message, TResult result = default)
    {
        return new ServiceResult<TResult> { Result = result, ErrorMessage = message, IsSuccess = false };
    }

    public static ServiceResult<TResult> NotFoundResult(string message)
    {
        return new ServiceResult<TResult> { ErrorMessage = message, IsSuccess = false, IsNotFoundError = true };
    }
    
    public static ServiceResult<TResult> FailureCodeResult(string messageCode, TResult result = default)
    {
        return new ServiceResult<TResult> { Result = result, MessageCode = messageCode, IsSuccess = false, IsFailedWithCode = true };
    }
}