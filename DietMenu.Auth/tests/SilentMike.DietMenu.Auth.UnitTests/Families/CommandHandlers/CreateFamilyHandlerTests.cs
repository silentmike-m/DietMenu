namespace SilentMike.DietMenu.Auth.UnitTests.Families.CommandHandlers;

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
    private readonly IMediator mediator = Substitute.For<IMediator>();
    private readonly IFamilyRepository repository = Substitute.For<IFamilyRepository>();

    [TestMethod]
    public async Task Should_Create_Family()
    {
        //GIVEN
        CreatedFamily? createdFamilyNotification = null;
        FamilyEntity? familyToSave = null;

        await this.mediator
            .Publish(Arg.Do<CreatedFamily>(notification => createdFamilyNotification = notification), Arg.Any<CancellationToken>());

        await this.repository
            .SaveAsync(Arg.Do<FamilyEntity>(family => familyToSave = family), Arg.Any<CancellationToken>());

        var request = new CreateFamily
        {
            Email = "family@domain.com",
            Id = Guid.NewGuid(),
            Name = "family name",
        };

        var handler = new CreateFamilyHandler(this.logger, this.mediator, this.repository);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        _ = this.repository.Received(1).SaveAsync(Arg.Any<FamilyEntity>(), Arg.Any<CancellationToken>());

        var expectedResult = new FamilyEntity(request.Id, request.Email, request.Name);

        familyToSave.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;

        _ = this.mediator.Received(1).Publish(Arg.Any<CreatedFamily>(), Arg.Any<CancellationToken>());

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
        var family = new FamilyEntity(Guid.NewGuid(), "family@domain.com", "family");

        this.repository
            .GetByIdAsync(family.Id, Arg.Any<CancellationToken>())
            .Returns(family);

        var request = new CreateFamily
        {
            Id = family.Id,
        };

        var handler = new CreateFamilyHandler(this.logger, this.mediator, this.repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyAlreadyExistsException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_ALREADY_EXISTS)
                .Where(exception => exception.Id == family.Id)
            ;

        _ = this.repository.Received(0).SaveAsync(Arg.Any<FamilyEntity>(), Arg.Any<CancellationToken>());

        _ = this.mediator.Received(0).Publish(Arg.Any<CreatedFamily>(), Arg.Any<CancellationToken>());
    }
}
