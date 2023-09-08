namespace SilentMike.DietMenu.Auth.UnitTests.Users.Validators;

using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Validators;
using SilentMike.DietMenu.Auth.Application.Users.ValueModels;

[TestClass]
public sealed class CreateUserValidatorTests
{
    [DataTestMethod, DataRow("userdomain.com"), DataRow("user@@domain"), DataRow("user@")]
    public async Task Should_Fail_Validation_When_Email_Has_Incorrect_Format(string email)
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = email,
            FamilyId = Guid.NewGuid(),
            FirstName = "John",
            Id = Guid.NewGuid(),
            LastName = "Wick",
            Password = "P@ssw0rd",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var validator = new CreateUserValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT_MESSAGE
            )
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Fail_Validation_When_Properties_Are_Empty_String()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "",
            FamilyId = Guid.NewGuid(),
            FirstName = "",
            Id = Guid.NewGuid(),
            LastName = "",
            Password = "",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var validator = new CreateUserValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .HaveCount(4)
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT_MESSAGE
            )
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME_MESSAGE
            )
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_LAST_NAME && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_LAST_NAME_MESSAGE
            )
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD_MESSAGE
            )
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Fail_Validation_When_Properties_Are_White_Spaces()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "  ",
            FamilyId = Guid.NewGuid(),
            FirstName = "  ",
            Id = Guid.NewGuid(),
            LastName = "  ",
            Password = "  ",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var validator = new CreateUserValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .HaveCount(4)
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMAIL_INVALID_FORMAT_MESSAGE
            )
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME_MESSAGE
            )
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_LAST_NAME && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_LAST_NAME_MESSAGE
            )
            .And
            .Contain(
                failure => failure.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD && failure.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD_MESSAGE
            )
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Pass_Validation_When_All_Properties_Are_Set()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "user@domain.com",
            FamilyId = Guid.NewGuid(),
            FirstName = "John",
            Id = Guid.NewGuid(),
            LastName = "Wick",
            Password = "P@ssw0rd",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var validator = new CreateUserValidator();

        //WHEN
        var result = await validator.ValidateAsync(request);

        //THEN
        result.Errors.Should()
            .BeEmpty()
            ;

        result.IsValid.Should()
            .BeTrue()
            ;
    }
}
