namespace Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<ApiError>? Errors { get; init; }

    public static ApiResponse<T> Ok(T data) => new() { Success = true, Data = data, Errors = null };

    public static ApiResponse<T> Fail(string message, string? field = null) => new()
    {
        Success = false,
        Data = default,
        Errors = new List<ApiError> { new(field, message) }
    };

    public static ApiResponse<T> Fail(IEnumerable<ApiError> errors) => new()
    {
        Success = false,
        Data = default,
        Errors = errors.ToList()
    };
}

public record ApiError(string? Field, string Message);
