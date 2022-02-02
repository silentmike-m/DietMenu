namespace SilentMike.DietMenu.Core.UnitTests.Hangfire;

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;

[TestClass]
public sealed class ImportFamilyLibrariesTests
{
    [TestMethod]
    public async Task ShouldImportLibraries()
    {
        //GIVEN
        var familyId = Guid.NewGuid();

        var fileProvider = new EmbeddedFileProvider(Assembly.GetAssembly(typeof(ImportFamilyLibraries))!);
        var logger = new Mock<ILogger<ImportFamilyLibraries>>();
        var mediator = new Mock<IMediator>();

        var job = new ImportFamilyLibraries(fileProvider, logger.Object, mediator.Object);

        //WHEN
        await job.Run(familyId);

        //tHEN
        mediator.Verify(i => i.Send(It.IsAny<ImportIngredientTypes>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(i => i.Send(It.IsAny<ImportMealTypes>(), It.IsAny<CancellationToken>()), Times.Once);

        mediator.Verify(i => i.Send(It.IsAny<ImportIngredients>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
