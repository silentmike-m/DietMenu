namespace SilentMike.DietMenu.Auth.Infrastructure.Families.QueryHandlers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

internal sealed class GetFamilyEmailHandler : IRequestHandler<GetFamilyEmail, string>
{
    private readonly IDietMenuDbContext context;
    private readonly ILogger<GetFamilyEmailHandler> logger;

    public GetFamilyEmailHandler(IDietMenuDbContext context, ILogger<GetFamilyEmailHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<string> Handle(GetFamilyEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to get family email");

        var family = await this.context.Families.SingleOrDefaultAsync(family => family.Id == request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        return family.Email;
    }
}
