namespace Domain.Category;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; }

    public string Slug { get; set; }

    public CategoryEntity(string name, string slug)
    {
        Name = name;
        Slug = slug;
    }
}