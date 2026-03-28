namespace Application.Services.Product.Request.CreateProduct;

public sealed record CreateProductRequest
{
    public string Name { get; init; }
    
    public decimal Price { get; init; }
    
    public int CategoryId { get; init; }
}
