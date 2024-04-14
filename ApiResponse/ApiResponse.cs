namespace GroupCoursework.ApiResponse
{
    public class ApiResponse<T> where T : class
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(string statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}
