namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Families.Events;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly ILogger<CreatedFamilyHandler> logger;

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {

    }
}
