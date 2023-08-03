namespace SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class GenerateResetPasswordTokenHandler : IRequestHandler<GenerateResetPasswordToken>
{
    private readonly ILogger<GenerateResetPasswordTokenHandler> logger;
    private readonly IPublisher mediator;
    private readonly UserManager<User> userManager;

    public GenerateResetPasswordTokenHandler(ILogger<GenerateResetPasswordTokenHandler> logger, IPublisher mediator, UserManager<User> userManager)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.userManager = userManager;
    }

    public async Task Handle(GenerateResetPasswordToken request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to generate reset user password token");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        //TODO: what if token is null or empty
        var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
        token ??= string.Empty;

        var notification = new GeneratedResetPasswordToken
        {
            Email = user.Email,
            Id = new Guid(user.Id),
            Token = token,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
