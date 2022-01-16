namespace DietMenu.Core.Infrastructure.Hangfire.EventHandlers;

using DietMenu.Core.Application.Families.Events;
using MediatR;
using Microsoft.Extensions.Logging;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly ILogger<CreatedFamilyHandler> logger;

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {

    }
}
