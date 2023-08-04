namespace SilentMike.DietMenu.Core.Application.Families.CommandHandlers;

using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

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
        this.logger.BeginScope(new Dictionary<string, object>
        {
            { "FamilyId", request.Id },
        });

        this.logger.LogInformation("Try to create family");

        var family = new FamilyEntity(request.Id);

        await this.repository.AddFamilyAsync(family, cancellationToken);

        var notification = new CreatedFamily
        {
            Id = request.Id,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
