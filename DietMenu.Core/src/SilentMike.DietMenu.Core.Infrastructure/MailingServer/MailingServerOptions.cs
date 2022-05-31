namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer;

internal sealed class MailingServerOptions
{
    public static readonly string SectionName = "MailingServer";
    public Uri BaseAddress { get; init; } = new("about:blank");
    public int RetryCount { get; init; } = 3;
    public int RetrySleepDurationInMilliSeconds { get; set; } = 5000;
    public int TimeoutInMilliSeconds { get; init; } = 30000;
}
