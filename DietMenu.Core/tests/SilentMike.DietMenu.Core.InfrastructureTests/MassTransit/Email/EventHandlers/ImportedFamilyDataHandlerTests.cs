namespace SilentMike.DietMenu.Core.InfrastructureTests.MassTransit.Email.EventHandlers;

using global::MassTransit;
using global::MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Infrastructure.Common.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Email.EventHandlers;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Email.Models;
using SilentMike.DietMenu.Shared.Email.Interfaces;
using SilentMike.DietMenu.Shared.Email.Models;
using ImportFamilyDataError = SilentMike.DietMenu.Core.Application.Families.Models.ImportFamilyDataError;
using ImportFamilyDataResult = SilentMike.DietMenu.Core.Application.Families.Models.ImportFamilyDataResult;

[TestClass]
public sealed class ImportedFamilyDataHandlerTests
{
    private readonly NullLogger<ImportedFamilyDataHandler> logger = new();

    [TestMethod]
    public async Task Should_Send_Imported_Family_DataMessage()
    {
        //GIVEN
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness()
            .BuildServiceProvider(true);

        var harness = provider.GetTestHarness();
        harness.TestTimeout = TimeSpan.FromSeconds(5);
        await harness.Start();

        var importedError = new ImportFamilyDataError("error", "message");

        var importedFamilyDataResult = new ImportFamilyDataResult
        {
            DataArea = IngredientTypeNames.Fruit,
            Errors = new List<ImportFamilyDataError>
            {
                importedError,
            },
        };

        var notification = new ImportedFamilyData
        {
            ErrorCode = "data error code",
            ErrorMessage = "data error message",
            FamilyId = Guid.NewGuid(),
            Results = new List<ImportFamilyDataResult>
            {
                importedFamilyDataResult,
            },
        };

        var handler = new ImportedFamilyDataHandler(harness.Bus, this.logger);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        (await harness.Published
                .Any<IEmailDataMessage>())
            .Should()
            .BeTrue()
            ;

        var expectedPayload = new ImportedFamilyDataPayload
        {
            ErrorCode = notification.ErrorCode,
            ErrorMessage = notification.ErrorMessage,
            FamilyId = notification.FamilyId,
            Results = new List<Shared.Email.Models.ImportFamilyDataResult>
            {
                new()
                {
                    DataArea = importedFamilyDataResult.DataArea,
                    Errors = new List<Shared.Email.Models.ImportFamilyDataError>
                    {
                        new()
                        {
                            Code = importedError.Code,
                            Message = importedError.Message,
                        },
                    },
                },
            },
        };

        var expectedMessage = new EmailDataMessage
        {
            Payload = expectedPayload.ToJson(),
            PayloadType = typeof(ImportedFamilyDataPayload).FullName!,
        };

        var messages = harness.Published
                .Select<IEmailDataMessage>()
                .ToList()
            ;

        messages.Should()
            .HaveCount(1)
            ;

        messages[0].Context.Message.Should()
            .BeEquivalentTo(expectedMessage)
            ;
    }
}
