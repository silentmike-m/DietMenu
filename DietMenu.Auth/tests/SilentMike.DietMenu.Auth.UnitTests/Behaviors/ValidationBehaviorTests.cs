namespace SilentMike.DietMenu.Auth.UnitTests.Behaviors;

using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Application.Common.Behaviors;
using SilentMike.DietMenu.Auth.Application.Families.Commands;
using SilentMike.DietMenu.Auth.Application.Families.Validators;
using ValidationException = SilentMike.DietMenu.Auth.Application.Exceptions.ValidationException;

[TestClass]
public sealed class ValidationBehaviorTests
{
    [TestMethod]
    public async Task Should_Pass_Validation_When_No_Validation_Errors()
    {
        //GIVEN
        var request = new CreateFamily
        {
            Email = "family@domain.com",
            Name = "family name",
        };

        var validator = new CreateFamilyValidator();

        var behavior = new ValidationBehavior<CreateFamily, Unit>(new[] { validator });

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Pass_Validation_When_No_Validators()
    {
        //GIVEN
        var request = new CreateFamily();

        var behavior = new ValidationBehavior<CreateFamily, Unit>(new List<IValidator<CreateFamily>>());

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Validation_Exception_On_Validation_Errors()
    {
        //GIVEN
        var request = new CreateFamily();

        var validator = new CreateFamilyValidator();

        var behavior = new ValidationBehavior<CreateFamily, Unit>(new[] { validator });

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
            ;
    }
}
