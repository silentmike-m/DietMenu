namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using IdentityOptions = SilentMike.DietMenu.Auth.Infrastructure.Identity.IdentityOptions;

internal sealed class CreateUserHandler : IRequestHandler<CreateUser>
{
    private readonly ILogger<CreateUserHandler> logger;
    private readonly IMediator mediator;
    private readonly IdentityOptions options;
    private readonly UserManager<DietMenuUser> userManager;


    public CreateUserHandler(
        ILogger<CreateUserHandler> logger,
        IMediator mediator,
        IOptions<IdentityOptions> options,
        UserManager<DietMenuUser> userManager)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.options = options.Value;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );
        this.logger.LogInformation("Try to create user");

        if (request.RegisterCode != this.options.RegisterCode)
        {
            throw new ArgumentException("Invalid register code");
        }

        var userId = Guid.NewGuid();

        var family = new DietMenuFamily
        {
            Id = userId,
            Name = request.Family,
        };

        var user = new DietMenuUser
        {
            Email = request.Email,
            Family = family,
            FirstName = request.FirstName,
            Id = userId.ToString(),
            LastName = request.LastName,
            UserName = request.Email,
        };

        var result = await this.userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var createdFamilyNotification = new CreatedFamily
            {
                Id = family.Id,
            };

            await this.mediator.Publish(createdFamilyNotification, cancellationToken);

            var createdUserNotification = new CreatedUser
            {
                Email = request.Email,
            };

            await this.mediator.Publish(createdUserNotification, cancellationToken);

            return await Task.FromResult(Unit.Value);
        }

        throw new CreateUserException(request.Email, result.Errors.First().Description);
    }
}
