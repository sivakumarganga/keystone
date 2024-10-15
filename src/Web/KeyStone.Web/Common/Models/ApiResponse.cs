namespace KeyStone.Web.Common.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string RequestId { get; set; }
        public Dictionary<string, object> ErrorResult { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();

        public static ApiResponse<T> OnFailure()
        {
            return new()
            {
                IsSuccess = false,
                Message = Constants.SomethingWentWrong
            };
        }
        public static ApiResponse<T> OnSuccess(T data)
        {
            return new()
            {
                IsSuccess = true,
                Data = data,
            };
        }
    }

}
