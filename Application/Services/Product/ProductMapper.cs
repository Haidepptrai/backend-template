using Application.Services.Product.Request.CreateProduct;
using Application.Services.Product.Response;
using Domain.Product;

namespace Application.Services.Product;

public static class ProductMapper
{
    public static GetProductsResponse ToGetProductsResponse(this ProductEntity product)
    {
        return new GetProductsResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name
        };
    }

    public static ProductEntity ToProductEntity(this CreateProductRequest request)
    {
        return new ProductEntity(request.Name, request.Price, request.CategoryId);
    }
}
