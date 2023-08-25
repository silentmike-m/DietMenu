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

        var familyImportService = new Mock<IFamilyMigrationService>();

        var job = new SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.Jobs.ImportFamilyData(familyImportService.Object);

        //WHEN
        await job.Run(familyId);

        //THEN
        familyImportService.Verify(service => service.ImportAsync(familyId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Should_Run_Import_Family_Data_Job()
    {
        //GIVEN
        var jobClient = new Mock<IBackgroundJobClient>();

        var request = new ImportFamilyData
        {
            FamilyId = Guid.NewGuid(),
        };

        var handler = new ImportFamilyDataHandler(jobClient.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        jobClient.Verify(backgroundJobClient => backgroundJobClient
                .Create(It.Is<Job>(job => job.Type == typeof(SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.Jobs.ImportFamilyData) && job.Args[0].ToString() == request.FamilyId.ToString()), It.IsAny<EnqueuedState>()), Times.Once
        );
    }
}
