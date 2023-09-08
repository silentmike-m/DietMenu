namespace SilentMike.DietMenu.Core.InfrastructureTests.Hangfire.Families.CommandHandlers;

using global::Hangfire;
using global::Hangfire.Common;
using global::Hangfire.States;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Infrastructure.Families.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.CommandHandlers;

[TestClass]
public sealed class ImportFamilyDataHandlerTests
{
    [TestMethod]
    public async Task Should_Run_Family_Migration()
    {
        //GIVEN
        var familyId = Guid.NewGuid();

        var familyImportService = Substitute.For<IFamilyMigrationService>();

        var job = new SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.Jobs.ImportFamilyData(familyImportService);

        //WHEN
        await job.Run(familyId);

        //THEN
        _ = familyImportService.Received(1).ImportAsync(familyId, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Should_Run_Import_Family_Data_Job()
    {
        //GIVEN
        var jobClient = Substitute.For<IBackgroundJobClient>();

        var request = new ImportFamilyData(Guid.NewGuid());

        var handler = new ImportFamilyDataHandler(jobClient);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        _ = jobClient.Received(1)
            .Create(Arg.Is<Job>(job => job.Type == typeof(SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.Jobs.ImportFamilyData) && job.Args[0].ToString() == request.FamilyId.ToString()), Arg.Any<EnqueuedState>());
    }
}
