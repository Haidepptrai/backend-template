namespace Application.Services.Category.Request;

public sealed record CreateCategoryRequest
{
    public string Name { get; init; }

    public string Slug { get; init; }
}