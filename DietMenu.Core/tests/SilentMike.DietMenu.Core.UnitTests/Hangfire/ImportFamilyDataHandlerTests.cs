namespace SilentMike.DietMenu.Core.UnitTests.Hangfire;

using System;
using System.Threading;
using global::Hangfire;
using global::Hangfire.Common;
using global::Hangfire.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.CommandHandlers;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;
using Request = SilentMike.DietMenu.Core.Application.Families.Commands.ImportFamilyData;

[TestClass]
public sealed class ImportFamilyDataHandlerTests
{
    [TestMethod]
    public void ShouldCreatedFamilyEventHandlerRunJob()
    {
        //GIVEN
        var jobClient = new Mock<IBackgroundJobClient>();

        var request = new Request
        {
            FamilyId = Guid.NewGuid(),
        };

        var requestHandler = new ImportFamilyDataHandler(jobClient.Object);

        //WHEN
        requestHandler.Handle(request, CancellationToken.None).Wait(CancellationToken.None);

        //THEN
        jobClient.Verify(x => x.Create(
            It.Is<Job>(job => job.Type == typeof(ImportFamilyData)
                              && job.Args[0].ToString() == request.FamilyId.ToString()
            ),
            It.IsAny<EnqueuedState>()));
    }
}
