namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class ResetPasswordHandler : IRequestHandler<ResetPassword>
{
    private readonly ILogger<ResetPasswordHandler> logger;
    private readonly UserManager<DietMenuUser> userManager;

    public ResetPasswordHandler(ILogger<ResetPasswordHandler> logger, UserManager<DietMenuUser> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(ResetPassword request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );
        this.logger.LogInformation("Try to reset password");

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            this.logger.LogError("User not found");
            return await Task.FromResult(Unit.Value);
        }

        var result = await this.userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (!result.Succeeded)
        {
            throw new ResetPasswordException(request.Email, result.Errors.First().Description);
        }

        return await Task.FromResult(Unit.Value);
    }
}
