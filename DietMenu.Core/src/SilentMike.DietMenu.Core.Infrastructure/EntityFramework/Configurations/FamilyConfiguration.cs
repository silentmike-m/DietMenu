namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

[ExcludeFromCodeCoverage]
internal sealed class FamilyConfiguration : IEntityTypeConfiguration<FamilyEntity>
{
    public void Configure(EntityTypeBuilder<FamilyEntity> builder)
    {
        builder.HasIndex(family => family.FamilyId)
            .IsUnique();

        builder
            .Property(e => e.IngredientsVersion)
            .HasConversion(
                ingredientsVersion => JsonSerializer.Serialize(ingredientsVersion, new JsonSerializerOptions()),
                ingredientsVersion => JsonSerializer.Deserialize<Dictionary<string, string>>(ingredientsVersion, new JsonSerializerOptions())!
            )
            .HasColumnType("json");
    }
}
