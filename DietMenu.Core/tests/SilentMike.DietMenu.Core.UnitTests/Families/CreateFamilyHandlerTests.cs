namespace SilentMike.DietMenu.Core.UnitTests.Families;

using SilentMike.DietMenu.Core.Application.Families.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;

[TestClass]
public sealed class CreateFamilyHandlerTests
{
    private readonly NullLogger<CreateFamilyHandler> logger = new();
    private readonly Mock<IPublisher> mediator = new();
    private readonly Mock<IFamilyRepository> repository = new();

    [TestMethod]
    public async Task Should_Create_Family()
    {
        //GIVEN
        CreatedFamily? createdFamilyNotification = null;
        FamilyEntity? familyToCreate = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()))
            .Callback<CreatedFamily, CancellationToken>((notification, _) => createdFamilyNotification = notification);

        this.repository
            .Setup(service => service.AddFamilyAsync(It.IsAny<FamilyEntity>(), It.IsAny<CancellationToken>()))
            .Callback<FamilyEntity, CancellationToken>((family, _) => familyToCreate = family);

        var request = new CreateFamily
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CreateFamilyHandler(this.logger, this.mediator.Object, this.repository.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.repository.Verify(service => service.AddFamilyAsync(It.IsAny<FamilyEntity>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedFamilyToCreate = new FamilyEntity(request.Id);

        familyToCreate.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedFamilyToCreate)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new CreatedFamily
        {
            Id = request.Id,
        };

        createdFamilyNotification.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Not_Publish_Notification_When_Create_Family_Fails()
    {
        //GIVEN
        var exception = new FamilyAlreadyExistsException(Guid.NewGuid());

        this.repository
            .Setup(service => service.AddFamilyAsync(It.IsAny<FamilyEntity>(), It.IsAny<CancellationToken>()))
            .Throws(exception);

        var request = new CreateFamily
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CreateFamilyHandler(this.logger, this.mediator.Object, this.repository.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyAlreadyExistsException>()
            ;

        this.repository.Verify(service => service.AddFamilyAsync(It.IsAny<FamilyEntity>(), It.IsAny<CancellationToken>()), Times.Once);

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
