namespace SilentMike.DietMenu.Core.InfrastructureTests.EPPlus.Extensions;

using SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;

[TestClass]
public sealed class EpplusExtensionsTests
{
    private const double DEFAULT_DOUBLE_VALUE = 22.25;

    [TestMethod]
    public void Should_Convert_Object_To_Double_On_To_Double()
    {
        //GIVEN
        const double value = 12;

        //WHEN
        var result = value.ToDouble(DEFAULT_DOUBLE_VALUE);

        //THEN
        result.Should()
            .Be(value)
            ;
    }

    [TestMethod]
    public void Should_Return_Default_Value_When_Object_Is_Not_Double_On_To_Double()
    {
        //GIVEN
        object value = "not double";

        //WHEN
        var result = value.ToDouble(DEFAULT_DOUBLE_VALUE);

        //THEN
        result.Should()
            .Be(DEFAULT_DOUBLE_VALUE)
            ;
    }

    [TestMethod]
    public void Should_Return_Default_Value_When_Object_Is_Null_On_To_Double()
    {
        //GIVEN
        object? value = null;

        //WHEN
        var result = value.ToDouble(DEFAULT_DOUBLE_VALUE);

        //THEN
        result.Should()
            .Be(DEFAULT_DOUBLE_VALUE)
            ;
    }

    [TestMethod]
    public void Should_Return_Empty_String_When_Object_Is_Null_On_To_Empty_String()
    {
        //GIVEN
        object? value = null;

        //WHEN
        var result = value.ToEmptyString();

        //THEN
        result.Should()
            .Be(string.Empty)
            ;
    }

    [TestMethod]
    public void Should_Return_Guid_When_Object_Is_Not_Guid_On_To_Non_Empty_Guid()
    {
        //GIVEN
        const int value = 22;

        //WHEN
        var result = value.ToNonEmptyGuid();

        //THEN
        result.Should()
            .NotBeEmpty()
            ;
    }

    [TestMethod]
    public void Should_Return_Guid_When_Object_Is_Not_Null_On_To_Non_Empty_Guid()
    {
        //GIVEN
        var value = Guid.NewGuid().ToString();

        //WHEN
        var result = value.ToNonEmptyGuid();

        //THEN
        result.Should()
            .Be(value)
            ;
    }

    [TestMethod]
    public void Should_Return_Guid_When_Object_Is_Null_On_To_Non_Empty_Guid()
    {
        //GIVEN
        object? value = null;

        //WHEN
        var result = value.ToNonEmptyGuid();

        //THEN
        result.Should()
            .NotBeEmpty()
            ;
    }

    [TestMethod]
    public void Should_Return_String_When_Object_Is_Not_Null_On_To_Empty_String()
    {
        //GIVEN
        const int value = 22;

        //WHEN
        var result = value.ToEmptyString();

        //THEN
        result.Should()
            .Be("22")
            ;
    }
}
