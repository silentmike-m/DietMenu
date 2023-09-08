namespace SilentMike.DietMenu.Auth.Application.Families.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common.Extensions;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Families.Commands;
using SilentMike.DietMenu.Auth.Application.Families.Events;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Services;

internal sealed class CreateFamilyHandler : IRequestHandler<CreateFamily>
{
    private readonly ILogger<CreateFamilyHandler> logger;
    private readonly IPublisher mediator;
    private readonly IFamilyRepository repository;

    public CreateFamilyHandler(ILogger<CreateFamilyHandler> logger, IPublisher mediator, IFamilyRepository repository)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.repository = repository;
    }

    public async Task Handle(CreateFamily request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.Id)
        );

        this.logger.LogInformation("Try to create family");

        var family = await this.repository.GetByIdAsync(request.Id, cancellationToken);

        if (family is not null)
        {
            throw new FamilyAlreadyExistsException(request.Id);
        }

        family = await this.repository.GetByNameAsync(request.Name, cancellationToken);

        if (family is not null)
        {
            throw new FamilyAlreadyExistsException(request.Name);
        }

        family = new FamilyEntity(request.Id, request.Email, request.Name);

        await this.repository.SaveAsync(family, cancellationToken);

        var notification = new CreatedFamily
        {
            Id = request.Id,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
