namespace DietMenu.Core.UnitTests.Families;

using System;
using System.Threading;
using System.Threading.Tasks;
using DietMenu.Core.Application.Common.Constants;
using DietMenu.Core.Application.Exceptions.Families;
using DietMenu.Core.Application.Families.CommandHandlers;
using DietMenu.Core.Application.Families.Commands;
using DietMenu.Core.Application.Families.Events;
using DietMenu.Core.Domain.Entities;
using DietMenu.Core.UnitTests.Services;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public sealed class CreateFamilyIfNotExistsHandlerTests
{
    private readonly Guid existingFamilyId = Guid.NewGuid();
    private readonly string existingFamilyName = "FamilyName";

    private readonly NullLogger<CreateFamilyIfNotExistsHandler> logger = new();
    private readonly Mock<IMediator> mediator = new();
    private readonly FamilyRepository repository = new();

    public CreateFamilyIfNotExistsHandlerTests()
    {
        var family = new FamilyEntity(this.existingFamilyId)
        {
            Name = this.existingFamilyName,
        };

        this.repository.Save(family);
    }

    [TestMethod]
    public async Task ShouldNotThrowExceptionAndNotCreateFamilyWithSameId()
    {
        //GIVEN
        var command = new CreateFamilyIfNotExists
        {
            Id = this.existingFamilyId,
        };

        var commandHandler = new CreateFamilyIfNotExistsHandler(this.logger, this.mediator.Object, this.repository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyAlreadyExistsWhenSameName()
    {
        //GIVEN
        var command = new CreateFamilyIfNotExists
        {
            Id = Guid.NewGuid(),
            Name = this.existingFamilyName,
        };

        var commandHandler = new CreateFamilyIfNotExistsHandler(this.logger, this.mediator.Object, this.repository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyAlreadyExistsException>()
                .Where(i => i.Code == ErrorCodes.FAMILY_ALREADY_EXISTS
                            && i.Id == Guid.Empty
                            && i.Message.Contains(this.existingFamilyName))
            ;
    }

    [TestMethod]
    public async Task ShouldCreateFamilyIfNotExists()
    {
        //GIVEN
        var command = new CreateFamilyIfNotExists
        {
            Id = Guid.NewGuid(),
            Name = "test_name",
        };

        var commandHandler = new CreateFamilyIfNotExistsHandler(this.logger, this.mediator.Object, this.repository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()), Times.Once);

        var family = await this.repository.Get(command.Id, CancellationToken.None);
        family.Should()
            .NotBeNull()
            ;
        family!.Name.Should()
            .Be(command.Name)
            ;
    }
}
