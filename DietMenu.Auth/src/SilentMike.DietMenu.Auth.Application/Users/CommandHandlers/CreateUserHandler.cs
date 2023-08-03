namespace SilentMike.DietMenu.Auth.Application.Users.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Services;

internal sealed class CreateUserHandler : IRequestHandler<CreateUser>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<CreateUserHandler> logger;
    private readonly IPublisher mediator;
    private readonly IUserRepository userRepository;

    public CreateUserHandler(IFamilyRepository familyRepository, ILogger<CreateUserHandler> logger, IPublisher mediator, IUserRepository userRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.userRepository = userRepository;
    }

    public async Task Handle(CreateUser request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", request.User.Id)
        );

        this.logger.LogInformation("Try to create user");

        var family = await this.familyRepository.GetByIdAsync(request.User.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.User.FamilyId);
        }

        var user = await this.userRepository.GetByIdAsync(request.User.Id, cancellationToken);

        if (user is not null)
        {
            throw new UserAlreadyExistsException(request.User.Id);
        }

        user = await this.userRepository.GetByEmailAsync(request.User.Email, cancellationToken);

        if (user is not null)
        {
            throw new UserAlreadyExistsException(request.User.Email);
        }

        user = new UserEntity(request.User.Email, request.User.FamilyId, request.User.FirstName, request.User.LastName, request.User.Id);

        await this.userRepository.CreateUserAsync(request.User.Password, user, cancellationToken);

        var notification = new CreatedUser
        {
            Email = user.Email,
            Id = user.Id,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
