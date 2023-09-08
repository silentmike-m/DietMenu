namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class GenerateEmailConfirmationTokenHandlerTests
{
    private static readonly Guid USER_ID = Guid.NewGuid();

    private readonly NullLogger<GenerateEmailConfirmationTokenHandler> logger = new();
    private readonly IPublisher mediator = Substitute.For<IPublisher>();

    [TestMethod]
    public async Task Should_Generate_Token()
    {
        //GIVEN
        GeneratedEmailConfirmationToken? generatedEmailConfirmationTokenNotification = null;

        await this.mediator
            .Publish(Arg.Do<GeneratedEmailConfirmationToken>(notification => generatedEmailConfirmationTokenNotification = notification), Arg.Any<CancellationToken>());

        const string token = "email_confirmation_token";

        var user = new User
        {
            Email = "user@domain.com",
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .FindByEmailAsync(user.Email)
                .Returns(user)
            )
            .With(manager => manager
                .GenerateEmailConfirmationTokenAsync(Arg.Any<User>())
                .Returns(token)
            )
            .Build();

        var request = new GenerateEmailConfirmationToken
        {
            Email = user.Email,
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator, userManager);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        _ = this.mediator.Received(1).Publish(Arg.Any<GeneratedEmailConfirmationToken>(), Arg.Any<CancellationToken>());

        var expectedNotification = new GeneratedEmailConfirmationToken
        {
            Email = user.Email,
            Id = USER_ID,
            Token = token,
        };

        generatedEmailConfirmationTokenNotification.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Not_Throw_Exception_When_Token_Is_Null()
    {
        //GIVEN
        var user = new User
        {
            Email = "user@domain.com",
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .FindByEmailAsync(user.Email)
                .Returns(user)
            )
            .With(manager => manager
                .GenerateEmailConfirmationTokenAsync(Arg.Any<User>())
                .Returns((string?)null)
            )
            .Build();

        var request = new GenerateEmailConfirmationToken
        {
            Email = user.Email,
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator, userManager);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
            .NotThrowAsync();

        _ = this.mediator.Received(0).Publish(Arg.Any<GeneratedEmailConfirmationToken>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Should_Throw_User_Not_Found_Exception_When_Missing_User()
    {
        //GIVEN
        var user = new User
        {
            Email = "user@domain.com",
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .FindByEmailAsync(user.Email)
                .Returns(user)
            )
            .Build();

        var request = new GenerateEmailConfirmationToken
        {
            Email = "fake@domain.com",
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator, userManager);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;

        _ = this.mediator.Received(0).Publish(Arg.Any<GeneratedEmailConfirmationToken>(), Arg.Any<CancellationToken>());
    }
}
