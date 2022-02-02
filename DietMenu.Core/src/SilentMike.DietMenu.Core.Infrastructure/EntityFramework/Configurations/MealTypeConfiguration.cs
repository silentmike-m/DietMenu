namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class MealTypeConfiguration : IEntityTypeConfiguration<MealTypeEntity>
{
    public void Configure(EntityTypeBuilder<MealTypeEntity> builder)
    {
        builder
            .HasIndex(i => new { i.FamilyId, i.InternalName })
            .IsUnique();

        builder
            .HasIndex(i => new { i.FamilyId, i.Name })
            .IsUnique();


        builder.HasOne(i => i.FamilyEntity)
            .WithMany()
            .IsRequired()
            .HasForeignKey("FamilyId");
    }
}
