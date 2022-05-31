namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework;

using SilentMike.DietMenu.Core.Application.Core.Commands;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

[TestClass]
public sealed class CoreMigrationServiceTests
{
    [TestMethod]
    public async Task ShouldSendImportCoreDataCommand()
    {
        ImportCoreData? importCoreDataCommand = null;

        //GIVEN
        var mediator = new Mock<IMediator>();

        mediator.Setup(i => i.Send(It.IsAny<ImportCoreData>(), CancellationToken.None))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => importCoreDataCommand = request as ImportCoreData);

        var migrationService = new CoreMigrationService(mediator.Object);

        //WHEN
        await migrationService.MigrateCoreAsync(CancellationToken.None);

        //THEN
        mediator.Verify(i => i.Send(It.IsAny<ImportCoreData>(), CancellationToken.None), Times.Once);

        importCoreDataCommand.Should()
            .NotBeNull()
            ;
        ;
        importCoreDataCommand!.ValidationOnly.Should()
            .BeFalse()
            ;
    }
}
