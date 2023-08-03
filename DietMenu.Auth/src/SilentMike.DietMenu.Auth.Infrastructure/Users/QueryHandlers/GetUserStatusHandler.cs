namespace SilentMike.DietMenu.Auth.Infrastructure.Users.QueryHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Application.Users.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using IdentityOptions = SilentMike.DietMenu.Auth.Infrastructure.Identity.IdentityOptions;

internal sealed class GetUserStatusHandler : IRequestHandler<GetUserStatus, UserStatus>
{
    private readonly IdentityOptions identityOptions;
    private readonly ILogger<GetUserStatusHandler> logger;
    private readonly UserManager<User> userManager;

    public GetUserStatusHandler(ILogger<GetUserStatusHandler> logger, IOptions<IdentityOptions> identityOptions, UserManager<User> userManager)
    {
        this.logger = logger;
        this.identityOptions = identityOptions.Value;
        this.userManager = userManager;
    }

    public async Task<UserStatus> Handle(GetUserStatus request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get user status");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var isLockedOut = user is
        {
            LockoutEnabled: true,
            LockoutEnd: not null,
        };

        var result = new UserStatus
        {
            IsEmailConfirmed = !this.identityOptions.RequireConfirmedAccount || user.EmailConfirmed,
            IsLockedOut = isLockedOut,
        };

        return result;
    }
}
