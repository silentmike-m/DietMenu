namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class FamilyConfiguration : IEntityTypeConfiguration<FamilyEntity>
{
    public void Configure(EntityTypeBuilder<FamilyEntity> builder)
    {
        builder.Property(family => family.Versions)
            .HasConversion(
                dictionary => JsonSerializer.Serialize(dictionary, new JsonSerializerOptions()),
                text => JsonSerializer.Deserialize<Dictionary<string, string>>(text, new JsonSerializerOptions())!
            )
            .HasMaxLength(4000);
    }
}
