namespace SilentMike.DietMenu.Core.UnitTests.Families;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Families.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class CreateFamilyHandlerTests : IDisposable
{
    private readonly Guid existingFamilyId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly NullLogger<CreateFamilyHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly FamilyRepository repository;

    public CreateFamilyHandlerTests()
    {
        var family = new FamilyEntity(this.existingFamilyId);

        this.factory = new DietMenuDbContextFactory(family);
        this.logger = new NullLogger<CreateFamilyHandler>();
        this.mediator = new Mock<IMediator>();
        this.repository = new FamilyRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyAlreadyExistsWhenSameId()
    {
        //GIVEN
        var command = new CreateFamily
        {
            Id = this.existingFamilyId,
        };

        var commandHandler = new CreateFamilyHandler(this.logger, this.mediator.Object, this.repository);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyAlreadyExistsException>()
                .Where(i => i.Code == ErrorCodes.FAMILY_ALREADY_EXISTS
                            && i.Id == command.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldCreateFamilyIfNotExists()
    {
        //GIVEN
        var command = new CreateFamily
        {
            Id = Guid.NewGuid(),
        };

        var commandHandler = new CreateFamilyHandler(this.logger, this.mediator.Object, this.repository);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<CreatedFamily>(), It.IsAny<CancellationToken>()), Times.Once);

        var family = await this.repository.GetAsync(command.Id, CancellationToken.None);
        family.Should()
            .NotBeNull()
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
