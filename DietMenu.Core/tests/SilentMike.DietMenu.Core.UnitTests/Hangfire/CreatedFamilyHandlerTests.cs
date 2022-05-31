namespace SilentMike.DietMenu.Core.UnitTests.Hangfire;

using global::Hangfire;
using global::Hangfire.Common;
using global::Hangfire.States;
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
        var handler = new CreatedFamilyHandler(jobClient.Object);

        //WHEN
        handler.Handle(assignEvent, CancellationToken.None).Wait(CancellationToken.None);

        //THEN
        jobClient.Verify(x => x.Create(
            It.Is<Job>(job => job.Type == typeof(ImportFamilyData)
                              && job.Args[0].ToString() == assignEvent.Id.ToString()
            ),
            It.IsAny<EnqueuedState>()));
    }
}
