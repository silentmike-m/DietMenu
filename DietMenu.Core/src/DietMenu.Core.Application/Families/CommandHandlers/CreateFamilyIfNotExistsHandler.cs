﻿namespace DietMenu.Core.Application.Families.CommandHandlers;

using DietMenu.Core.Application.Exceptions.Families;
using DietMenu.Core.Application.Families.Commands;
using DietMenu.Core.Application.Families.Events;
using DietMenu.Core.Domain.Entities;
using DietMenu.Core.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

internal sealed class CreateFamilyIfNotExistsHandler : IRequestHandler<CreateFamilyIfNotExists>
{
    private readonly ILogger<CreateFamilyIfNotExistsHandler> logger;
    private readonly IMediator mediator;
    private readonly IFamilyRepository repository;

    public CreateFamilyIfNotExistsHandler(ILogger<CreateFamilyIfNotExistsHandler> logger, IMediator mediator, IFamilyRepository repository)
        => (this.logger, this.mediator, this.repository) = (logger, mediator, repository);

    public async Task<Unit> Handle(CreateFamilyIfNotExists request, CancellationToken cancellationToken)
    {
        this.logger.BeginScope(new Dictionary<string, object>
        {
            {"FamilyId", request.Id},
        });

        this.logger.LogInformation("Try to create family if not exists");

        var family = await this.repository.Get(request.Id, cancellationToken);

        if (family is null)
        {
            family = await this.repository.Get(request.Name, cancellationToken);

            if (family is not null)
            {
                throw new FamilyAlreadyExistsException(request.Name);
            }

            family = new FamilyEntity(request.Id)
            {
                Name = request.Name,
            };

            await this.repository.Save(family, cancellationToken);

            var notification = new CreatedFamily
            {
                Id = request.Id,
            };

            await this.mediator.Publish(notification, cancellationToken);
        }

        return await Task.FromResult(Unit.Value);
    }
}
