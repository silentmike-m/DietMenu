namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

[ExcludeFromCodeCoverage]
internal sealed class IngredientConfiguration : IEntityTypeConfiguration<IngredientEntity>
{
    public void Configure(EntityTypeBuilder<IngredientEntity> builder)
    {
        builder.HasIndex(ingredient => new { ingredient.FamilyId, ingredient.IngredientId })
            .IsUnique();
    }
}
