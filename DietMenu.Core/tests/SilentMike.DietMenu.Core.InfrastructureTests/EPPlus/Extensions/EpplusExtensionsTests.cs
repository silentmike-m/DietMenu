namespace SilentMike.DietMenu.Core.InfrastructureTests.EPPlus.Extensions;

using SilentMike.DietMenu.Core.Infrastructure.EPPlus.Extensions;

[TestClass]
public sealed class EpplusExtensionsTests
{
    private const decimal DEFAULT_VALUE = 22.25m;

    [TestMethod]
    public void Should_Convert_Object_To_Decimal()
    {
        //GIVEN
        const decimal value = 12m;

        //WHEN
        var result = value.ToDecimal(DEFAULT_VALUE);

        //THEN
        result.Should()
            .Be(value)
            ;
    }

    [TestMethod]
    public void Should_Return_Default_Value_When_Object_Is_Not_Decimal()
    {
        //GIVEN
        object value = "not decimal";

        //WHEN
        var result = value.ToDecimal(DEFAULT_VALUE);

        //THEN
        result.Should()
            .Be(DEFAULT_VALUE)
            ;
    }

    [TestMethod]
    public void Should_Return_Default_Value_When_Object_Is_Null()
    {
        //GIVEN
        object? value = null;

        //WHEN
        var result = value.ToDecimal(DEFAULT_VALUE);

        //THEN
        result.Should()
            .Be(DEFAULT_VALUE)
            ;
    }
}
