namespace SilentMike.DietMenu.Core.Application.Families.CommandHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class CreateFamilyHandler : IRequestHandler<CreateFamily>
{
    private readonly ILogger<CreateFamilyHandler> logger;
    private readonly IMediator mediator;
    private readonly IFamilyRepository repository;

    public CreateFamilyHandler(ILogger<CreateFamilyHandler> logger, IMediator mediator, IFamilyRepository repository)
        => (this.logger, this.mediator, this.repository) = (logger, mediator, repository);

    public async Task<Unit> Handle(CreateFamily request, CancellationToken cancellationToken)
    {
        this.logger.BeginScope(new Dictionary<string, object>
        {
            {"FamilyId", request.Id},
        });

        this.logger.LogInformation("Try to create family");

        var family = await this.repository.Get(request.Id, cancellationToken);

        if (family is not null)
        {
            throw new FamilyAlreadyExistsException(request.Id);
        }

        family = new FamilyEntity(request.Id);

        await this.repository.Save(family, cancellationToken);

        var notification = new CreatedFamily
        {
            Id = request.Id,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
