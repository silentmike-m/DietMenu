namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Configurations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;

[ExcludeFromCodeCoverage]
internal sealed class IngredientTypeRowConfiguration : IEntityTypeConfiguration<IngredientTypeRow>
{
    public void Configure(EntityTypeBuilder<IngredientTypeRow> builder)
    {

    }
}
