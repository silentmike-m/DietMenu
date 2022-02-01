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
            .HasIndex(i => new { i.FamilyId, i.InternalName })
            .IsUnique();

        builder
            .HasIndex(i => new { i.FamilyId, i.Name })
            .IsUnique();

        builder.HasOne(i => i.Type)
            .WithMany()
            .HasForeignKey(i => i.TypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
