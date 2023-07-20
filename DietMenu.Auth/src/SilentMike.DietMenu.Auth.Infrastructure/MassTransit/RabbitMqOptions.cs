namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit;

internal sealed class RabbitMqOptions
{
    public static readonly string SECTION_NAME = "RabbitMQ";
    public string HostName { get; set; } = "localhost";
    public string Password { get; set; } = "guest";
    public ushort Port { get; set; } = 5672;
    public string User { get; set; } = "guest";
    public bool UseSsl { get; set; } = default;
    public string VirtualHost { get; set; } = "/";
}
