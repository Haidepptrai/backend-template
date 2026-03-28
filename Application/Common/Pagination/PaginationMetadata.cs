namespace Application.Common.Pagination;

public record PaginationMetadata
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int TotalItems { get; init; }
    public int ItemsPerPage { get; init; }
}
