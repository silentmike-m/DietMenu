namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class MealTypeConfiguration : IEntityTypeConfiguration<MealTypeEntity>
{
    public void Configure(EntityTypeBuilder<MealTypeEntity> builder)
    {
        builder.Property(i => i.FamilyId).IsRequired();
        builder.Property(i => i.InternalName).IsRequired();
        builder.Property(i => i.Name).IsRequired();
        builder.Property(i => i.Order).IsRequired().HasDefaultValue(1);

        builder.HasIndex(i => i.MealTypeId);
        builder.HasIndex(i => i.InternalName).IsUnique();
        builder.HasIndex(i => i.Name).IsUnique();
    }
}
