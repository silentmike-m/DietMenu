namespace DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using DietMenu.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class FamilyConfiguration : IEntityTypeConfiguration<FamilyEntity>
{
    public void Configure(EntityTypeBuilder<FamilyEntity> builder)
    {
        builder.Property(i => i.Name).IsRequired().HasMaxLength(100);

        builder.HasKey(i => i.Identifier);

        builder.HasIndex(i => i.Id).IsUnique();
        builder.HasIndex(i => i.Name).IsUnique();
    }
}
