using Application.Common.Pagination;
using Application.Services.Product.Request;
using Application.Services.Product.Request.CreateProduct;
using Application.Services.Product.Request.UpdateProduct;
using Application.Services.Product.Response;
using Domain.Product;

namespace Application.Services.Product;

public interface IProductService
{
    Task<ProductEntity> CreateProductAsync(CreateProductRequest request);

    Task<PagedResult<GetProductsResponse>> GetProductsAsync(GetProductsQuery query);

    Task<ProductEntity?> GetProductByIdAsync(int id);

    Task<ProductEntity?> UpdateProductAsync(int id, UpdateProductRequest request);

    Task<bool> DeleteProductAsync(int id);
}