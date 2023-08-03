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
public sealed class GeneratedResetPasswordTokenHandlerTests
{
    private const string DEFAULT_CLIENT_URI = "https://client.domain.com";
    private const string ISSUER_URI = "https://auth.domain.com";
    private const string TOKEN = "reset_password_token";
    private const string URL = "reset.password.domain.com";

    private readonly Mock<IIdentityPageUrlService> identityPageUrlService = new();
    private readonly IOptions<IdentityServerOptions> identityServerOptions;
    private readonly NullLogger<GeneratedResetPasswordTokenHandler> logger = new();

    public GeneratedResetPasswordTokenHandlerTests()
        => this.identityServerOptions = Options.Create(new IdentityServerOptions
        {
            DefaultClientUri = DEFAULT_CLIENT_URI,
            IssuerUri = ISSUER_URI,
        });

    [TestMethod]
    public async Task Should_Send_Reset_User_Password_Email_Message()
    {
        //GIVEN
        this.identityPageUrlService
            .Setup(service => service.GetResetUserPasswordPageUrl(new Uri(ISSUER_URI), new Uri(DEFAULT_CLIENT_URI), TOKEN))
            .Returns(URL);

        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness()
            .BuildServiceProvider(true);

        var harness = provider.GetTestHarness();
        harness.TestTimeout = TimeSpan.FromSeconds(5);
        await harness.Start();

        var notification = new GeneratedResetPasswordToken
        {
            Email = "user@domain.com",
            Id = Guid.NewGuid(),
            Token = TOKEN,
        };

        var handler = new GeneratedResetPasswordTokenHandler(harness.Bus, this.identityPageUrlService.Object, this.identityServerOptions, this.logger);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        (await harness.Published
                .Any<IEmailDataMessage>())
            .Should()
            .BeTrue()
            ;

        var expectedPayload = new ResetUserPasswordEmailPayload
        {
            Email = notification.Email,
            Url = URL,
        };

        var expectedMessage = new EmailDataMessage
        {
            Payload = expectedPayload.ToJson(),
            PayloadType = typeof(ResetUserPasswordEmailPayload).FullName!,
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
