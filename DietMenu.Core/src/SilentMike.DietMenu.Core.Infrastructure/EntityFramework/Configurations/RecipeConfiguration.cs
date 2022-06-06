namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class RecipeConfiguration : IEntityTypeConfiguration<RecipeEntity>
{
    public void Configure(EntityTypeBuilder<RecipeEntity> builder)
    {
        builder.HasOne(recipe => recipe.FamilyEntity)
            .WithMany()
            .IsRequired()
            .HasForeignKey("FamilyId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(recipe => recipe.MealType)
            .WithMany()
            .IsRequired()
            .HasForeignKey("MealTypeId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
