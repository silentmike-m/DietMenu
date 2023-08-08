namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.CommandHandlers;

using global::Hangfire;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using Job = SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.Jobs.ImportFamilyData;

internal sealed class ImportFamilyDataHandler : IRequestHandler<ImportFamilyData>
{
    private readonly IBackgroundJobClient jobClient;

    public ImportFamilyDataHandler(IBackgroundJobClient jobClient)
        => this.jobClient = jobClient;

    public async Task Handle(ImportFamilyData request, CancellationToken cancellationToken)
    {
        this.jobClient.Enqueue<Job>(i => i.Run(request.FamilyId));

        await Task.CompletedTask;
    }
}
