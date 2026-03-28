namespace Application.Services.Product.Response;

public sealed record GetProductsResponse
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public decimal Price { get; init; }
    
    public int CategoryId { get; init; }
    
    // Nice addition following CRUD standards
    public string CategoryName { get; init; } 
}
