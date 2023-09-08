namespace SilentMike.DietMenu.Auth.UnitTests.Families.Validators;

using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Families.Commands;
using SilentMike.DietMenu.Auth.Application.Families.Validators;

[TestClass]
public sealed class CreateFamilyValidatorTests
{
    [DataTestMethod, DataRow(""), DataRow("   "), DataRow("family domain com")]
    public async Task Should_Fail_Validation_When_Email_Has_Wrong_Format(string email)
    {
        //GIVEN
        var request = new CreateFamily
        {
            Email = email,
            Name = "family",
        };

        var validator = new CreateFamilyValidator();

        //WHEN
        var result = await validator.ValidateAsync(request, CancellationToken.None);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(error =>
                error.ErrorCode == ValidationErrorCodes.CREATE_FAMILY_EMAIL_INVALID_FORMAT
                && error.ErrorMessage == ValidationErrorCodes.CREATE_FAMILY_EMAIL_INVALID_FORMAT_MESSAGE
            )
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [DataTestMethod, DataRow(""), DataRow("   ")]
    public async Task Should_Fail_Validation_When_Name_Is_Empty_Spaces(string name)
    {
        //GIVEN
        var request = new CreateFamily
        {
            Email = "family@domain.com",
            Name = name,
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
            Email = "family@domain.com",
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
