using Application.Common.Pagination;
using Application.Persistence;
using Application.Services.Product.Exceptions;
using Application.Services.Product.Request;
using Application.Services.Product.Request.CreateProduct;
using Application.Services.Product.Request.UpdateProduct;
using Application.Services.Product.Response;
using Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<ProductEntity> CreateProductAsync(CreateProductRequest request)
    {
        var product = request.ToProductEntity();

        _context.Product.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    // READ ALL
    public async Task<PagedResult<GetProductsResponse>> GetProductsAsync(GetProductsQuery query)
    {
        var dbQuery = _context.Product.AsNoTracking();

        var totalItems = await dbQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        // Technique: Query by ID first to optimize performance
        var productIds = await dbQuery
            .OrderBy(p => p.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => p.Id)
            .ToListAsync();

        var products = new List<GetProductsResponse>();
        if (productIds.Any())
        {
            products = await _context.Product
                .Include(p => p.Category)
                .Where(p => productIds.Contains(p.Id))
                .OrderBy(p => p.Id)
                .AsNoTracking()
                .Select(product => product.ToGetProductsResponse())
                .ToListAsync();
        }

        return new PagedResult<GetProductsResponse>
        {
            Data = products,
            Metadata = new PaginationMetadata
            {
                CurrentPage = query.Page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                ItemsPerPage = query.PageSize
            }
        };
    }

    // READ BY ID
    public async Task<ProductEntity?> GetProductByIdAsync(int id)
    {
        var product = await _context.Product
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new ProductNotFoundException(id);

        return product;
    }

    // UPDATE
    public async Task<ProductEntity?> UpdateProductAsync(int id, UpdateProductRequest request)
    {
        var product = await _context.Product.FindAsync(id);

        if (product == null)
            throw new ProductNotFoundException(id);

        product.Name = request.Name;
        product.Price = request.Price;
        product.CategoryId = request.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return product;
    }

    // DELETE
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Product.FindAsync(id);

        if (product == null)
            throw new ProductNotFoundException(id);

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}
