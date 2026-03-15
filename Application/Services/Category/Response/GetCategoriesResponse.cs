namespace Application.Services.Category.Response;

public record GetCategoriesResponse
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Slug { get; init; }
}