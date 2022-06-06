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
            .HasIndex(type => new { type.FamilyId, type.InternalName })
            .IsUnique();

        builder
            .HasIndex(type => new { type.FamilyId, type.Name })
            .IsUnique();

        builder.HasOne(type => type.FamilyEntity)
            .WithMany()
            .IsRequired()
            .HasForeignKey("FamilyId");
    }
}
