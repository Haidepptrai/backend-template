using Application.Persistence;
using Application.Services.Category.Request;
using Application.Services.Category.Response;
using Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Category;

public class CategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<CategoryEntity> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = request.ToCategoryEntity();

        _context.Category.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    // READ ALL
    public async Task<List<GetCategoriesResponse>> GetCategoriesAsync()
    {
        return await _context.Category
            .AsNoTracking()
            .Select(category => category.ToGetCategoriesResponse())
            .ToListAsync();
    }

    // READ BY ID
    public async Task<CategoryEntity?> GetCategoryByIdAsync(int id)
    {
        return await _context.Category
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // UPDATE
    public async Task<CategoryEntity?> UpdateCategoryAsync(int id, CategoryEntity updatedCategory)
    {
        var category = await _context.Category.FindAsync(id);

        if (category == null)
            return null;

        category.Name = updatedCategory.Name;
        category.Slug = updatedCategory.Slug;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return category;
    }

    // DELETE
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Category.FindAsync(id);

        if (category == null)
            return false;

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
}