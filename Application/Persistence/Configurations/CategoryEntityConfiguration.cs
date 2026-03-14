using Domain.Category;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistence.Configurations;

public class CategoryEntityConfiguration : BaseEntityConfiguration<CategoryEntity>
{
    public override void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(c => c.Slug)
            .IsUnique();
    }
}