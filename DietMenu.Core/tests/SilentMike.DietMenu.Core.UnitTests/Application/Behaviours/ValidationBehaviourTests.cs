namespace SilentMike.DietMenu.Core.UnitTests.Application.Behaviours;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Validators;
using SilentMike.DietMenu.Core.Application.Auth.ViewModels;
using SilentMike.DietMenu.Core.Application.Common.Behaviours;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ValidationException = SilentMike.DietMenu.Core.Application.Exceptions.ValidationException;

[TestClass]
public sealed class ValidationBehaviourTests
{
    [TestMethod]
    public async Task ShouldThrowValidationExceptionWhenAreErrors()
    {
        //GIVEN
        var request = new CreateUser
        {
            CreateCode = "CreateCode",
            User = new UserToCreate
            {
                Email = "test.test.pl",
                FamilyName = "Family Name",
                FirstName = "First Name",
                Id = Guid.NewGuid(),
                LastName = "Last Name",
                Password = "Password",
                UserName = "User Name",
            },
        };

        var validators = new List<IValidator<CreateUser>>
        {
            new CreateUserValidator(),
        };

        var behaviour = new ValidationBehaviour<CreateUser, Unit>(validators);

        //WHEN
        Func<Task<Unit>> action = async () => await behaviour.Handle(request, CancellationToken.None, () => Task.FromResult(Unit.Value));

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i => i.Code == ErrorCodes.VALIDATION_FAILED)
            ;
    }

    [TestMethod]
    public async Task ShouldNotThrowValidationExceptionWhenNoErrors()
    {
        //GIVEN
        var request = new CreateUser
        {
            CreateCode = "CreateCode",
            User = new UserToCreate
            {
                Email = "test@test.pl",
                FamilyName = "Family Name",
                FirstName = "First Name",
                Id = Guid.NewGuid(),
                LastName = "Last Name",
                Password = "Password",
                UserName = "User Name",
            },
        };

        var validators = new List<IValidator<CreateUser>>
        {
            new CreateUserValidator(),
        };

        var behaviour = new ValidationBehaviour<CreateUser, Unit>(validators);

        //WHEN
        Func<Task<Unit>> action = async () => await behaviour.Handle(request, CancellationToken.None, () => Task.FromResult(Unit.Value));

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }
}
