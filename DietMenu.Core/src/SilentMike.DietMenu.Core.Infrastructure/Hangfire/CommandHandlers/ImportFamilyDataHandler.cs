namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.CommandHandlers;

using System.Threading;
using System.Threading.Tasks;
using global::Hangfire;
using MediatR;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using Job = SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs.ImportFamilyData;

internal sealed class ImportFamilyDataHandler : IRequestHandler<ImportFamilyData>
{
    private readonly IBackgroundJobClient jobClient;

    public ImportFamilyDataHandler(IBackgroundJobClient jobClient)
        => this.jobClient = jobClient;

    public async Task<Unit> Handle(ImportFamilyData request, CancellationToken cancellationToken)
    {
        this.jobClient.Enqueue<Job>(i => i.Run(request.FamilyId));
        return await Task.FromResult(Unit.Value);
    }
}
