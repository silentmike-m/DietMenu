namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class IngredientConfiguration : IEntityTypeConfiguration<IngredientEntity>
{
    public void Configure(EntityTypeBuilder<IngredientEntity> builder)
    {
        builder
            .HasIndex(ingredient => new { ingredient.FamilyId, ingredient.InternalName })
            .IsUnique();

        builder
            .HasIndex(ingredient => new { ingredient.FamilyId, ingredient.Name })
            .IsUnique();

        builder.HasOne(ingredient => ingredient.FamilyEntity)
            .WithMany()
            .IsRequired()
            .HasForeignKey("FamilyId");

        builder.HasOne(ingredient => ingredient.Type)
            .WithMany()
            .HasForeignKey(i => i.TypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
