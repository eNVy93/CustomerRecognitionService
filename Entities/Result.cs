namespace CustomerRecognitionService.Entities
{
    public class Result<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        private Result(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static Result<T> Ok(T data, string message = "Success") =>
            new(true, message, data);

        public static Result<T> Ok(string message = "Success") =>
            new(true, message);

        public static Result<T> Fail(string message) =>
            new(false, message);
        public static Result<T> Fail(string message, T data) =>
            new(false, message, data);
    }

}
