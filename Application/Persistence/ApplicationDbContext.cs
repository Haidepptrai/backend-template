using Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Application.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CategoryEntity> Category { get; set; }
}