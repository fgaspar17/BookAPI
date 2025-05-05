namespace BookAPI.Services.Result;

public class ResultService<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public ServiceErrorCode ErrorCode { get; set; }

    public static ResultService<T> Fail(string message, ServiceErrorCode errorCode)
    => new() { Success = false, ErrorMessage = message, ErrorCode = errorCode };

    public static ResultService<T> Ok(T data)
        => new() { Success = true, Data = data };

}

public enum ServiceErrorCode
{
    None,
    NotFound,
    ValidationError,
    Conflict,
    Unauthorized,
    InternalError
}
