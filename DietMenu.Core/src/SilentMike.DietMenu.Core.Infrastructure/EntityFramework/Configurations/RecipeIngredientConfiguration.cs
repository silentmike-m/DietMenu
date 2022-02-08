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
        builder.HasOne(i => i.Recipe)
            .WithMany()
            .IsRequired()
            .HasForeignKey("RecipeId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Ingredient)
            .WithMany()
            .IsRequired()
            .HasForeignKey("IngredientId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
