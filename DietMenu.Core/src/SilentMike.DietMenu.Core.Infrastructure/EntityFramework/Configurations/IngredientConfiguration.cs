namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

[ExcludeFromCodeCoverage]
internal sealed class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasIndex(p => new { p.FamilyId, p.InternalId })
            .IsUnique();

        builder.HasIndex(p => new { p.FamilyId, p.InternalName })
            .IsUnique();
    }
}
