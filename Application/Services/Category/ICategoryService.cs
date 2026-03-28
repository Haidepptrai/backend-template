using Application.Services.Category.Request.CreateCategory;
using Application.Services.Category.Response;
using Domain.Category;

namespace Application.Services.Category;

public interface ICategoryService
{
    Task<CategoryEntity> CreateCategoryAsync(CreateCategoryRequest request);

    Task<List<GetCategoriesResponse>> GetCategoriesAsync();

    Task<CategoryEntity?> GetCategoryByIdAsync(int id);

    Task<CategoryEntity?> UpdateCategoryAsync(int id, CategoryEntity updatedCategory);

    Task<bool> DeleteCategoryAsync(int id);
}