namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit.Email.EventHandlers;

using FluentAssertions;
using global::MassTransit;
using global::MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    private const string ISSUER_URI = "https://auth.domain.com";

    private readonly Mock<IIdentityPageUrlService> identityPageUrlService = new();
    private readonly IOptions<IdentityServerOptions> identityServerOptions;
    private readonly NullLogger<GeneratedEmailConfirmationTokenHandler> logger = new();

    public GeneratedEmailConfirmationTokenHandlerTests()
        => this.identityServerOptions = Options.Create(new IdentityServerOptions
        {
            IssuerUri = ISSUER_URI,
        });

    [TestMethod]
    public async Task Should_Send_Confirm_User_Email_Message()
    {
        //GIVEN
        const string token = "confirm_token";
        const string url = "confirm.domain.com";
        var userId = Guid.NewGuid();

        this.identityPageUrlService
            .Setup(service => service.GetConfirmUserEmailPageUrl(new Uri(ISSUER_URI), token, userId))
            .Returns(url);

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
            Token = token,
        };

        var handler = new GeneratedEmailConfirmationTokenHandler(harness.Bus, this.identityPageUrlService.Object, this.identityServerOptions, this.logger);

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
            Url = url,
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
