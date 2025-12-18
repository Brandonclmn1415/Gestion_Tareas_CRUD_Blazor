namespace SIG_T.Domain.DTO.API;

/// <summary>
/// Generic API response wrapper
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class ApiResponseDTO<T>
{
    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The data returned by the operation
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error message if the operation failed
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// List of validation errors
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// Timestamp of the response
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful response
    /// </summary>
    /// <param name="data">The data to return</param>
    /// <param name="message">Optional success message</param>
    /// <returns>Successful API response</returns>
    public static ApiResponseDTO<T> SuccessResult(T data, string? message = null)
    {
        return new ApiResponseDTO<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    /// <summary>
    /// Creates an error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">List of validation errors</param>
    /// <returns>Error API response</returns>
    public static ApiResponseDTO<T> ErrorResult(string message, IEnumerable<string>? errors = null)
    {
        return new ApiResponseDTO<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

/// <summary>
/// API response for operations that don't return data
/// </summary>
public class ApiResponseDTO : ApiResponseDTO<object>
{
    /// <summary>
    /// Creates a successful response without data
    /// </summary>
    /// <param name="message">Optional success message</param>
    /// <returns>Successful API response</returns>
    public static ApiResponseDTO SuccessResult(string? message = null)
    {
        return new ApiResponseDTO
        {
            Success = true,
            Message = message
        };
    }

    /// <summary>
    /// Creates an error response without data
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">List of validation errors</param>
    /// <returns>Error API response</returns>
    public static ApiResponseDTO ErrorResult(string message, IEnumerable<string>? errors = null)
    {
        return new ApiResponseDTO
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

/// <summary>
/// Response for async operations (HTTP 202 Accepted)
/// </summary>
public class AsyncOperationResponseDTO
{
    /// <summary>
    /// Operation ID for tracking
    /// </summary>
    public string OperationId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Status message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Estimated completion time
    /// </summary>
    public DateTime EstimatedCompletion { get; set; }

    /// <summary>
    /// Timestamp when the operation was queued
    /// </summary>
    public DateTime QueuedAt { get; set; } = DateTime.UtcNow;

    public AsyncOperationResponseDTO()
    {
        EstimatedCompletion = DateTime.UtcNow.AddMinutes(5);
    }

    public AsyncOperationResponseDTO(string message) : this()
    {
        Message = message;
    }
}