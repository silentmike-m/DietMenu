namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        builder.HasKey(entity => entity.Key);

        builder.HasIndex(entity => entity.Id)
            .IsUnique();

        builder.HasIndex(entity => entity.Name)
            .IsUnique();
    }
}
