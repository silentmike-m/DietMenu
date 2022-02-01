namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class IngredientTypeConfiguration : IEntityTypeConfiguration<IngredientTypeEntity>
{
    public void Configure(EntityTypeBuilder<IngredientTypeEntity> builder)
    {
        builder
            .HasIndex(i => new { i.FamilyId, i.InternalName })
            .IsUnique();

        builder
            .HasIndex(i => new { i.FamilyId, i.Name })
            .IsUnique();
    }
}
