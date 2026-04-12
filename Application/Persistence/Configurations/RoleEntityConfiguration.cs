using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistence.Configurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        // Seed data
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new RoleEntity { Id = 1, Name = RoleConstants.Admin, CreatedAt = seedDate, UpdatedAt = seedDate },
            new RoleEntity { Id = 2, Name = RoleConstants.User, CreatedAt = seedDate, UpdatedAt = seedDate }
        );
    }
}
