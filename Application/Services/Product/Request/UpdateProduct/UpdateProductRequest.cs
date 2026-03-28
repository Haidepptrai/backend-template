namespace Application.Services.Product.Request.UpdateProduct;

public sealed record UpdateProductRequest
{
    public string Name { get; init; }
    
    public decimal Price { get; init; }
    
    public int CategoryId { get; init; }
}
