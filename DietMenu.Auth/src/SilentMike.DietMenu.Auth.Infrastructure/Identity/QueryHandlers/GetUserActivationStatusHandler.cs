namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.QueryHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Models;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class GetUserActivationStatusHandler : IRequestHandler<GetUserActivationStatus, UserActivationStatus>
{
    private readonly ILogger<GetUserActivationStatusHandler> logger;
    private readonly UserManager<DietMenuUser> userManager;

    public GetUserActivationStatusHandler(ILogger<GetUserActivationStatusHandler> logger, UserManager<DietMenuUser> userManager)
        => (this.logger, this.userManager) = (logger, userManager);

    public async Task<UserActivationStatus> Handle(GetUserActivationStatus request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );
        this.logger.LogInformation("Try to get user activation status");

        var dietMenuUser = await this.userManager.FindByEmailAsync(request.Email);

        if (dietMenuUser is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var result = new UserActivationStatus
        {
            IsActive = dietMenuUser.EmailConfirmed,
        };

        return result;
    }
}
