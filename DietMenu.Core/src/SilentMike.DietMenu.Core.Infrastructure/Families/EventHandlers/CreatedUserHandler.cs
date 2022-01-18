namespace SilentMike.DietMenu.Core.Infrastructure.Families.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Application.Users.Events;

internal sealed class CreatedUserHandler : INotificationHandler<CreatedUser>
{
    private readonly ILogger<CreatedUserHandler> logger;
    private readonly IMediator mediator;

    public CreatedUserHandler(ILogger<CreatedUserHandler> logger, IMediator mediator)
        => (this.logger, this.mediator) = (logger, mediator);

    public async Task Handle(CreatedUser notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("UserId", notification.Id),
            ("FamilyId", notification.FamilyId),
            ("FamilyName", notification.FamilyName)
        );

        var createFamilyRequest = new CreateFamilyIfNotExists
        {
            Id = notification.FamilyId,
            Name = notification.FamilyName,
        };

        _ = await this.mediator.Send(createFamilyRequest, cancellationToken);
    }
}
