namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class FamilyConfiguration : IEntityTypeConfiguration<FamilyEntity>
{
    public void Configure(EntityTypeBuilder<FamilyEntity> builder)
    {
        builder.Property(i => i.Name).IsRequired().HasMaxLength(100);

        builder.HasIndex(i => i.FamilyId).IsUnique();
        builder.HasIndex(i => i.Name).IsUnique();

        builder.HasMany(i => i.MealTypes);
    }
}
