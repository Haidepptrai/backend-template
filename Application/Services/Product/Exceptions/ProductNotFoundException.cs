using Application.Common.Exceptions;

namespace Application.Services.Product.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(int id) : base("Product", id)
    {
    }
}
