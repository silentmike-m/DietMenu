namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredientEntity>
{
    public void Configure(EntityTypeBuilder<RecipeIngredientEntity> builder)
    {
        builder.HasOne(ingredient => ingredient.Recipe)
            .WithMany()
            .IsRequired()
            .HasForeignKey("RecipeId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ingredient => ingredient.Ingredient)
            .WithMany()
            .IsRequired()
            .HasForeignKey("IngredientId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
