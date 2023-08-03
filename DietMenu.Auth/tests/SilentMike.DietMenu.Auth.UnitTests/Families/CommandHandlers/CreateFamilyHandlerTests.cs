namespace SilentMike.DietMenu.Auth.UnitTests.Families.CommandHandlers;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Families.CommandHandlers;
using SilentMike.DietMenu.Auth.Application.Families.Commands;
using SilentMike.DietMenu.Auth.Application.Families.Events;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Services;

[TestClass]
public sealed class CreateFamilyHandlerTests
{
    private readonly NullLogger<CreateFamilyHandler> logger = new();
    private readonly Mock<IMediator> mediator = new();
    private readonly Mock<IFamilyRepository> repository = new();

    [TestMethod]
    public async Task Should_Create_Family()
    {
        //GIVEN
        CreatedFamily? createdFamilyNotification = null;
        FamilyEntity? familyToSave = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()))
            .Callback<CreatedFamily, CancellationToken>((notification, _) => createdFamilyNotification = notification);

        this.repository
            .Setup(service => service.SaveAsync(It.IsAny<FamilyEntity>(), It.IsAny<CancellationToken>()))
            .Callback<FamilyEntity, CancellationToken>((family, _) => familyToSave = family);

        var request = new CreateFamily
        {
            Id = Guid.NewGuid(),
            Name = "family name",
        };

        var handler = new CreateFamilyHandler(this.logger, this.mediator.Object, this.repository.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.repository.Verify(service => service.SaveAsync(It.IsAny<FamilyEntity>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedResult = new FamilyEntity(request.Id, request.Name);

        familyToSave.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
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
    public async Task Should_Throw_Family_Already_Exists_Exception_When_Family_With_Same_Id_Already_Exists()
    {
        //GIVEN
        var family = new FamilyEntity(Guid.NewGuid(), "family");

        this.repository
            .Setup(service => service.GetByIdAsync(family.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(family);

        var request = new CreateFamily
        {
            Id = family.Id,
        };

        var handler = new CreateFamilyHandler(this.logger, this.mediator.Object, this.repository.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyAlreadyExistsException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_ALREADY_EXISTS)
                .Where(exception => exception.Id == family.Id)
            ;

        this.repository.Verify(service => service.SaveAsync(It.IsAny<FamilyEntity>(), It.IsAny<CancellationToken>()), Times.Never);

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
