using Application.Services.Category.Request;
using Application.Services.Category.Response;
using Domain.Category;

namespace Application.Services.Category;

public static class CategoryMapper
{
    public static GetCategoriesResponse ToGetCategoriesResponse(this CategoryEntity category)
    {
        return new GetCategoriesResponse
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug
        };
    }

    public static CategoryEntity ToCategoryEntity(this CreateCategoryRequest request)
    {
        return new CategoryEntity(request.Name, request.Slug);
    }
}