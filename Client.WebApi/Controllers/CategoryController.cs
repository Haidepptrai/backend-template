using Application.Persistence;
using Domain.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryEntity category)
    {
        try
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // READ ALL
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var categories = await _context.Category.ToListAsync();
            return Ok(categories);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // READ BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        try
        {
            var category = await _context.Category
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryEntity updatedCategory)
    {
        try
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
                return NotFound();

            category.Name = updatedCategory.Name;
            category.Slug = updatedCategory.Slug;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(category);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Category deleted successfully.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}