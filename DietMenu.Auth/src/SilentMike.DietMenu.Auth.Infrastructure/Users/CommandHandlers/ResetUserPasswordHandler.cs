namespace SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class ResetUserPasswordHandler : IRequestHandler<ResetUserPassword>
{
    private readonly ILogger<ResetUserPasswordHandler> logger;
    private readonly UserManager<User> userManager;

    public ResetUserPasswordHandler(ILogger<ResetUserPasswordHandler> logger, UserManager<User> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task Handle(ResetUserPassword request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to reset user password");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var resetPasswordResult = await this.userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (resetPasswordResult.Succeeded is false)
        {
            throw new ResetUserPasswordException(new Guid(user.Id), resetPasswordResult.Errors.First().Description);
        }
    }
}
