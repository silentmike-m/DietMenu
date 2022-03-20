namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Domain.Entities;

[ExcludeFromCodeCoverage]
internal sealed class CoreConfiguration : IEntityTypeConfiguration<CoreEntity>
{
    public void Configure(EntityTypeBuilder<CoreEntity> builder)
    {
        builder.Property(u => u.Versions)
            .HasConversion(
                dictionary => JsonSerializer.Serialize(dictionary, new JsonSerializerOptions()),
                text => JsonSerializer.Deserialize<Dictionary<string, string>>(text, new JsonSerializerOptions())!
            )
            .HasMaxLength(4000)
            .IsRequired();
    }
}
