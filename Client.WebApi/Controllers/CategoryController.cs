using Application.Common.ResponseWrapper;
using Application.Services.Category;
using Application.Services.Category.Request.CreateCategory;
using Application.Services.Category.Response;
using Domain.Category;
using Microsoft.AspNetCore.Mvc;

namespace Client.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // READ ALL
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<GetCategoriesResponse>>>> GetCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();

        return Ok(ApiResponse<List<GetCategoriesResponse>>.Success(categories, "Categories retrieved successfully."));
    }

    // READ BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryEntity>>> GetCategory(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        return Ok(ApiResponse<CategoryEntity>.Success(category, "Category retrieved successfully."));
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryEntity>>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateCategoryAsync(request);
        return Ok(ApiResponse<CategoryEntity>.Success(category, "Category created successfully."));
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryEntity>>> UpdateCategory(int id, [FromBody] CategoryEntity updatedCategory)
    {
        var category = await _categoryService.UpdateCategoryAsync(id, updatedCategory);

        return Ok(ApiResponse<CategoryEntity>.Success(category, "Category updated successfully."));
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);

        return Ok(ApiResponse<string>.Success(null, "Category deleted successfully."));
    }
}