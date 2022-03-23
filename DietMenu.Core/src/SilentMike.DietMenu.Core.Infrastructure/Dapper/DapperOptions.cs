namespace SilentMike.DietMenu.Core.Infrastructure.Dapper;

internal sealed class DapperOptions
{
    public static readonly string SectionName = "Dapper";
    public string ConnectionString { get; set; } = string.Empty;
}
