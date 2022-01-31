namespace SilentMike.DietMenu.Core.UnitTests.Hangfire;

using System;
using System.Threading;
using global::Hangfire;
using global::Hangfire.Common;
using global::Hangfire.States;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.EventHandlers;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;

[TestClass]
public sealed class CreatedFamilyHandlerTests
{
    [TestMethod]
    public void ShouldCreatedFamilyEventHandlerRunJob()
    {
        //GIVEN
        var jobClient = new Mock<IBackgroundJobClient>();

        var assignEvent = new CreatedFamily
        {
            Id = Guid.NewGuid(),
        };
        var handlerLogger = new NullLogger<CreatedFamilyHandler>();
        var handler = new CreatedFamilyHandler(jobClient.Object, handlerLogger);

        //WHEN
        handler.Handle(assignEvent, CancellationToken.None).Wait(CancellationToken.None);

        //THEN
        jobClient.Verify(x => x.Create(
            It.Is<Job>(job => job.Type == typeof(ImportFamilyLibraries)
                              && job.Args[0].ToString() == assignEvent.Id.ToString()
            ),
            It.IsAny<EnqueuedState>()));
    }
}
