namespace SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class UpdateUserLastLoginDateHandler : IRequestHandler<UpdateUserLastLoginDate>
{
    private readonly IDateTimeService dateTimeService;
    private readonly ILogger<UpdateUserLastLoginDateHandler> logger;
    private readonly UserManager<User> userManager;

    public UpdateUserLastLoginDateHandler(IDateTimeService dateTimeService, ILogger<UpdateUserLastLoginDateHandler> logger, UserManager<User> userManager)
    {
        this.dateTimeService = dateTimeService;
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task Handle(UpdateUserLastLoginDate request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to update user last login date");

        var userIdString = request.UserId.ToString();

        var user = await this.userManager.FindByIdAsync(userIdString);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var lastLoginDate = this.dateTimeService.GetNow().ToTrimmedDateTime();

        user.LastLogin = lastLoginDate;

        await this.userManager.UpdateAsync(user);
    }
}
