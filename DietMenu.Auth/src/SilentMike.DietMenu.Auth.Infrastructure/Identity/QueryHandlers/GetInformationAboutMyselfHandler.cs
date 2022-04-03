namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Application.Users.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class GetInformationAboutMyselfHandler : IRequestHandler<GetInformationAboutMyself, User>
{
    private readonly ILogger<GetInformationAboutMyselfHandler> logger;
    private readonly UserManager<DietMenuUser> userManager;

    public GetInformationAboutMyselfHandler(ILogger<GetInformationAboutMyselfHandler> logger, UserManager<DietMenuUser> userManager)
        => (this.logger, this.userManager) = (logger, userManager);

    public async Task<User> Handle(GetInformationAboutMyself request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", request.UserId)
        );
        this.logger.LogInformation("Try to get information about my self");

        var userId = request.UserId.ToString();

        var dietMenuUser = await this.userManager.FindByIdAsync(userId);

        if (dietMenuUser is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var user = new User
        {
            Id = dietMenuUser.Id,
            Email = dietMenuUser.Email,
            FamilyId = dietMenuUser.FamilyId,
            FirstName = dietMenuUser.FirstName,
            IsActivated = dietMenuUser.EmailConfirmed,
            LastName = dietMenuUser.LastName,
            PhoneNumber = dietMenuUser.PhoneNumber,
        };

        return user;
    }
}
