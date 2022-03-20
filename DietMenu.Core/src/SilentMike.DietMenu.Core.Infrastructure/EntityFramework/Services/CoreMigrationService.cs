namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using MediatR;
using SilentMike.DietMenu.Core.Application.Core.Commands;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

internal sealed class CoreMigrationService : ICoreMigrationService
{
    private readonly IMediator mediator;

    public CoreMigrationService(IMediator mediator)
        => this.mediator = mediator;

    public async Task MigrateCoreAsync(CancellationToken cancellationToken = default)
    {
        var request = new ImportCoreData
        {
            ValidationOnly = false,
        };

        _ = await this.mediator.Send(request, cancellationToken);
    }
}
