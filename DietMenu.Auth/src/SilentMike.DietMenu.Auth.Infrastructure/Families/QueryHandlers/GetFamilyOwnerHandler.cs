namespace SilentMike.DietMenu.Auth.Infrastructure.Families.QueryHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Application.Families.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class GetFamilyOwnerHandler : IRequestHandler<GetFamilyOwner, FamilyOwner>
{
    private readonly IDietMenuDbContext context;
    private readonly ILogger<GetFamilyOwnerHandler> logger;
    private readonly UserManager<User> userManager;

    public GetFamilyOwnerHandler(IDietMenuDbContext context, ILogger<GetFamilyOwnerHandler> logger, UserManager<User> userManager)
    {
        this.context = context;
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task<FamilyOwner> Handle(GetFamilyOwner request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to get family owner");

        var family = await this.context.Families.SingleOrDefaultAsync(family => family.Id == request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var user = this.userManager.Users.FirstOrDefault(user => user.FamilyKey == family.Key);

        if (user is null)
        {
            throw new FamilyOwnerNotFoundException(request.FamilyId);
        }

        var owner = new FamilyOwner
        {
            Email = user.Email,
            UserId = new Guid(user.Id),
        };

        return owner;
    }
}
