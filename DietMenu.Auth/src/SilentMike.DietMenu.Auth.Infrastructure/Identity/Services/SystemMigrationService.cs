namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;
using SilentMike.DietMenu.Auth.Domain.Enums;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using IdentityOptions = SilentMike.DietMenu.Auth.Infrastructure.Identity.IdentityOptions;

internal sealed class SystemMigrationService : ISystemMigrationService
{
    private const string DEFAULT_PORTFOLIO_NAME = "System";
    private const string DEFAULT_SYSTEM_USER_EMAIL = "system@domain.com";
    private const string DEFAULT_SYSTEM_USER_FIRST_NAME = "John";
    private const string DEFAULT_SYSTEM_USER_LAST_NAME = "Smith";
    private const string DEFAULT_SYSTEM_USER_PASSWORD = "P@ssw0rd";

    private readonly IDietMenuDbContext context;
    private readonly IdentityOptions identityOptions;
    private readonly ILogger<SystemMigrationService> logger;
    private readonly UserManager<User> userManager;

    public SystemMigrationService(IDietMenuDbContext context, IOptions<IdentityOptions> identityOptions, ILogger<SystemMigrationService> logger, UserManager<User> userManager)
    {
        this.context = context;
        this.identityOptions = identityOptions.Value;
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task MigrateSystemAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Try to migrate system");

        var family = await this.CreateSystemFamily(cancellationToken);

        await this.CreateSystemUser(family, cancellationToken);
    }

    private async Task<Family> CreateSystemFamily(CancellationToken cancellationToken)
    {
        var family = await this.context.Families.SingleOrDefaultAsync(family => family.Id == FamilyIds.SYSTEM_FAMILY_ID, cancellationToken);

        if (family is null)
        {
            family = new Family
            {
                Id = FamilyIds.SYSTEM_FAMILY_ID,
                Name = DEFAULT_PORTFOLIO_NAME,
            };

            this.context.Families.Add(family);
            await this.context.SaveChangesAsync(cancellationToken);
        }

        return family;
    }

    private async Task CreateSystemUser(Family family, CancellationToken cancellationToken)
    {
        var email = this.identityOptions.SystemUserEmail;

        if (string.IsNullOrWhiteSpace(email))
        {
            this.logger.LogInformation("System user email is empty. Using default email");
            email = DEFAULT_SYSTEM_USER_EMAIL;
        }

        var password = this.identityOptions.SystemUserPassword;

        if (string.IsNullOrWhiteSpace(password))
        {
            this.logger.LogInformation("System user password is empty. Using default password");
            password = DEFAULT_SYSTEM_USER_PASSWORD;
        }

        var user = await this.userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                EmailConfirmed = true,
                FirstName = DEFAULT_SYSTEM_USER_FIRST_NAME,
                LastName = DEFAULT_SYSTEM_USER_LAST_NAME,
                Family = family,
                Role = UserRole.System,
                UserName = email,
            };

            _ = await this.userManager.CreateAsync(user, password);
        }
    }
}
