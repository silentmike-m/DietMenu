namespace SilentMike.DietMenu.Auth.UnitTests.Users.EventHandlers;

using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Users.EventHandlers;

[TestClass]
public sealed class CreatedUserHandlerTests
{
    private readonly NullLogger<CreatedUserHandler> logger = new();
    private readonly ISender mediator = Substitute.For<ISender>();

    [TestMethod]
    public async Task Should_Not_Sent_Generate_Email_Confirmation_Token_When_Not_Require_Account_Confirmation()
    {
        //GIVEN
        var options = new IdentityOptions
        {
            RequireConfirmedAccount = false,
        };

        var identityOptions = Options.Create(options);

        var notification = new CreatedUser
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CreatedUserHandler(identityOptions, this.logger, this.mediator);

        //THEN
        await handler.Handle(notification, CancellationToken.None);

        _ = this.mediator.Received(0).Send(Arg.Any<GenerateEmailConfirmationToken>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Should_Sent_Generate_Email_Confirmation_Token_When_Require_Account_Confirmation()
    {
        //GIVEN
        GenerateEmailConfirmationToken? generateEmailConfirmationTokenRequest = null;

        await this.mediator
            .Send(Arg.Do<GenerateEmailConfirmationToken>(request => generateEmailConfirmationTokenRequest = request), Arg.Any<CancellationToken>());

        var options = new IdentityOptions
        {
            RequireConfirmedAccount = true,
        };

        var identityOptions = Options.Create(options);

        var notification = new CreatedUser
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CreatedUserHandler(identityOptions, this.logger, this.mediator);

        //THEN
        await handler.Handle(notification, CancellationToken.None);

        _ = this.mediator.Received(1).Send(Arg.Any<GenerateEmailConfirmationToken>(), Arg.Any<CancellationToken>());

        var expectedRequest = new GenerateEmailConfirmationToken
        {
            Email = notification.Email,
        };

        generateEmailConfirmationTokenRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }
}
