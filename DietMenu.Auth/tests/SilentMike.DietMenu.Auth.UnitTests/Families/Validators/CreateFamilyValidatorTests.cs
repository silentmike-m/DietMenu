namespace SilentMike.DietMenu.Auth.UnitTests.Families.Validators;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Families.Commands;
using SilentMike.DietMenu.Auth.Application.Families.Validators;

[TestClass]
public sealed class CreateFamilyValidatorTests
{
    [TestMethod]
    public async Task Should_Fail_Validation_When_Name_Is_Empty_Spaces()
    {
        //GIVEN
        var request = new CreateFamily
        {
            Name = "   ",
        };

        var validator = new CreateFamilyValidator();

        //WHEN
        var result = await validator.ValidateAsync(request, CancellationToken.None);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(error =>
                error.ErrorCode == ValidationErrorCodes.CREATE_FAMILY_EMPTY_NAME
                && error.ErrorMessage == ValidationErrorCodes.CREATE_FAMILY_EMPTY_NAME_MESSAGE
            )
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Fail_Validation_When_Name_Is_Empty_String()
    {
        //GIVEN
        var request = new CreateFamily
        {
            Name = "",
        };

        var validator = new CreateFamilyValidator();

        //WHEN
        var result = await validator.ValidateAsync(request, CancellationToken.None);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(error =>
                error.ErrorCode == ValidationErrorCodes.CREATE_FAMILY_EMPTY_NAME
                && error.ErrorMessage == ValidationErrorCodes.CREATE_FAMILY_EMPTY_NAME_MESSAGE
            )
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Pass_Validation_When_Name_Is_Not_Empty()
    {
        //GIVEN
        var request = new CreateFamily
        {
            Name = "family name",
        };

        var validator = new CreateFamilyValidator();

        //WHEN
        var result = await validator.ValidateAsync(request, CancellationToken.None);

        //THEN
        result.Errors.Should()
            .BeEmpty()
            ;

        result.IsValid.Should()
            .BeTrue()
            ;
    }
}
