namespace SilentMike.DietMenu.Mailing.Infrastructure.Smtp;

internal sealed class SmtpOptions
{
    public static readonly string SectionName = "Smtp";
    public string From { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; } = default;
    public string User { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = default;
}
