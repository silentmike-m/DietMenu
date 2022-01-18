namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit;

internal sealed class RabbitMqOptions
{
    public static readonly string SectionName = "RabbitMQ";
    public string HostName { get; set; } = string.Empty;
    public Uri Server { get; set; } = new Uri("about:blank");
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
