namespace SilentMike.DietMenu.Mailing.WebApi;

internal sealed class CorsOptions
{
    public static readonly string SECTION_NAME = "Cors";
    public string[] AllowedHeaders { get; set; } = Array.Empty<string>();
    public string[] AllowedMethods { get; set; } = Array.Empty<string>();
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}
