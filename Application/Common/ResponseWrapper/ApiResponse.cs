using Application.Common.Pagination;

namespace Application.Common.ResponseWrapper;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public PaginationMetadata? Metadata { get; set; }

    public ApiResponse()
    {
    }

    public static ApiResponse<T> Success(T data, string message = null)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Message = message
        };
    }

    public static ApiResponse<T> SuccessWithMetaData(T data, PaginationMetadata metadata, string message = null)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Metadata = metadata,
            Message = message
        };
    }
}