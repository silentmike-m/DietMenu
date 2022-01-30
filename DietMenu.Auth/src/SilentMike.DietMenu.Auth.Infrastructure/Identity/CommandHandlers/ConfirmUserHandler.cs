namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class ConfirmUserHandler : IRequestHandler<ConfirmUser>
{
    private readonly ILogger<ConfirmUserHandler> logger;
    private readonly UserManager<DietMenuUser> userManager;

    public ConfirmUserHandler(ILogger<ConfirmUserHandler> logger, UserManager<DietMenuUser> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(ConfirmUser request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", request.Id)
        );
        this.logger.LogInformation("Try to confirm user");

        var user = await this.userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
        {
            throw new UserNotFoundException(request.Id);
        }

        var result = await this.userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
        {
            throw new ArgumentException("Token not valid.");
        }

        return await Task.FromResult(Unit.Value);
    }
}
