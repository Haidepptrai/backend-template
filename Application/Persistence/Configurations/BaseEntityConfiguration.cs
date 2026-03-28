using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistence.Configurations;

public abstract class BaseEntityConfiguration<T>
    : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'");

        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'");
    }
}