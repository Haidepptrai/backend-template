using Application.Common.Exceptions;

namespace Application.Services.Category.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(int id) : base("Category", id)
    {
    }
}
