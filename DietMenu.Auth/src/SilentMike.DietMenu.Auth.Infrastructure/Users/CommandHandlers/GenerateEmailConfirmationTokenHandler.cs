namespace SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class GenerateEmailConfirmationTokenHandler : IRequestHandler<GenerateEmailConfirmationToken>
{
    private readonly ILogger<GenerateEmailConfirmationTokenHandler> logger;
    private readonly IPublisher mediator;
    private readonly UserManager<User> userManager;

    public GenerateEmailConfirmationTokenHandler(ILogger<GenerateEmailConfirmationTokenHandler> logger, IPublisher mediator, UserManager<User> userManager)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.userManager = userManager;
    }

    public async Task Handle(GenerateEmailConfirmationToken request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to generate email confirmation token");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

        if (token is null)
        {
            this.logger.LogError("Email confirmation token is null");

            return;
        }

        var notification = new GeneratedEmailConfirmationToken
        {
            Email = user.Email,
            Id = new Guid(user.Id),
            Token = token,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
