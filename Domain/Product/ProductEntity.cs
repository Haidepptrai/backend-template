using Domain.Category;

namespace Domain.Product;

public class ProductEntity : BaseEntity
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; }

    public ProductEntity(string name, decimal price, int categoryId)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
    }
}
