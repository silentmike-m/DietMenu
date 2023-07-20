namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(entity => entity.Key);

        builder.HasIndex(entity => entity.Id)
            .IsUnique();
    }
}
