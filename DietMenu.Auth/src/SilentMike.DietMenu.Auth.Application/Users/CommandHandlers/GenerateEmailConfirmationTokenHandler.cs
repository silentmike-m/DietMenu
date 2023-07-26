namespace SilentMike.DietMenu.Auth.Application.Users.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Domain.Services;

internal sealed class GenerateEmailConfirmationTokenHandler : IRequestHandler<GenerateEmailConfirmationToken>
{
    private readonly ILogger<GenerateEmailConfirmationTokenHandler> logger;
    private readonly IPublisher mediator;
    private readonly IUserService userService;

    public GenerateEmailConfirmationTokenHandler(ILogger<GenerateEmailConfirmationTokenHandler> logger, IPublisher mediator, IUserService userService)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.userService = userService;
    }

    public async Task Handle(GenerateEmailConfirmationToken request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            ("UserId", request.Id)
        );

        this.logger.LogInformation("Try to generate email confirmation token");

        var user = await this.userService.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException(request.Id);
        }

        var token = await this.userService.GenerateEmailConfirmationTokenAsync(user, cancellationToken);

        if (token is null)
        {
            this.logger.LogError("Token is null");

            return;
        }

        var notification = new GeneratedEmailConfirmationToken
        {
            Email = user.Email,
            Id = user.Id,
            Token = token,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
