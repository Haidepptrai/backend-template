namespace Application.Common.Pagination;

public record PagedResult<T>
{
    public required List<T> Data { get; init; }
    public required PaginationMetadata Metadata { get; init; }
}
