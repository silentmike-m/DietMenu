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
        builder
            .HasMany(i => i.MealTypes)
            .WithOne(i => i.FamilyEntity)
            .IsRequired()
            .HasForeignKey(i => i.FamilyId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder
            .HasMany(i => i.Ingredients)
            .WithOne(i => i.FamilyEntity)
            .IsRequired()
            .HasForeignKey(i => i.FamilyId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder
            .HasMany(i => i.IngredientTypes)
            .WithOne(i => i.FamilyEntity)
            .IsRequired()
            .HasForeignKey(i => i.FamilyId)
            .OnDelete(DeleteBehavior.Cascade)
            ;
    }
}
