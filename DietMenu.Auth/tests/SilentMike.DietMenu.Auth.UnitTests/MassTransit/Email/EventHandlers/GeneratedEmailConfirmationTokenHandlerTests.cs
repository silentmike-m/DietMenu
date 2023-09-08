namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit.Email.EventHandlers;

using global::MassTransit;
using global::MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Extensions;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Email.EventHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Email.Models;
using SilentMike.DietMenu.Shared.Email.Interfaces;
using SilentMike.DietMenu.Shared.Email.Models;

[TestClass]
public sealed class GeneratedEmailConfirmationTokenHandlerTests
{
    private const string DEFAULT_CLIENT_URI = "https://client.domain.com";
    private const string ISSUER_URI = "https://auth.domain.com";
    private const string TOKEN = "confirm_token";
    private const string URL = "confirm.domain.com";

    private readonly IIdentityPageUrlService identityPageUrlService = Substitute.For<IIdentityPageUrlService>();

    private readonly IOptions<IdentityServerOptions> identityServerOptions = Options.Create(new IdentityServerOptions
    {
        DefaultClientUri = DEFAULT_CLIENT_URI,
        IssuerUri = ISSUER_URI,
    });

    private readonly NullLogger<GeneratedEmailConfirmationTokenHandler> logger = new();

    [TestMethod]
    public async Task Should_Send_Confirm_User_Email_Message()
    {
        //GIVEN
        var userId = Guid.NewGuid();

        this.identityPageUrlService
            .GetConfirmUserEmailPageUrl(new Uri(ISSUER_URI), new Uri(DEFAULT_CLIENT_URI), Arg.Any<string>(), userId)
            .Returns(URL);

        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness()
            .BuildServiceProvider(true);

        var harness = provider.GetTestHarness();
        harness.TestTimeout = TimeSpan.FromSeconds(5);
        await harness.Start();

        var notification = new GeneratedEmailConfirmationToken
        {
            Email = "user@domain.com",
            Id = userId,
            Token = TOKEN,
        };

        var handler = new GeneratedEmailConfirmationTokenHandler(harness.Bus, this.identityPageUrlService, this.identityServerOptions, this.logger);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        (await harness.Published
                .Any<IEmailDataMessage>())
            .Should()
            .BeTrue()
            ;

        var expectedPayload = new ConfirmUserEmailPayload
        {
            Email = notification.Email,
            Url = URL,
        };

        var expectedMessage = new EmailDataMessage
        {
            Payload = expectedPayload.ToJson(),
            PayloadType = typeof(ConfirmUserEmailPayload).FullName!,
        };

        var messages = harness.Published
                .Select<IEmailDataMessage>()
                .ToList()
            ;

        messages.Should()
            .HaveCount(1)
            ;

        messages[0].Context.Message.Should()
            .BeEquivalentTo(expectedMessage)
            ;
    }
}
