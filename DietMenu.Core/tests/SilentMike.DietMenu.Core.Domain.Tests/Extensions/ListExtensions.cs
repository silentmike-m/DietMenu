namespace SilentMike.DietMenu.Core.Domain.Tests.Extensions;

using SilentMike.DietMenu.Core.Domain.Extensions;

[TestClass]
public sealed class ListExtensions
{
    [TestMethod]
    public void Should_Return_Null_When_List_Not_Contains_String_On_Get_Ignore_Case()
    {
        //GIVEN
        const string value = "string";

        var list = new List<string>
        {
            value,
        };

        //WHEN
        var result = list.GetIgnoreCase("test");

        //THEN
        result.Should()
            .BeNull()
            ;
    }

    [TestMethod]
    public void Should_Return_String_When_List_Contains_Lower_String_On_Get_Ignore_Case()
    {
        //GIVEN
        const string value = "string";

        var list = new List<string>
        {
            value,
        };

        //WHEN
        var result = list.GetIgnoreCase(value.ToUpper());

        //THEN
        result.Should()
            .Be(value)
            ;
    }

    [TestMethod]
    public void Should_Return_String_When_List_Contains_tring_On_Get_Ignore_Case()
    {
        //GIVEN
        const string value = "string";

        var list = new List<string>
        {
            value,
        };

        //WHEN
        var result = list.GetIgnoreCase(value);

        //THEN
        result.Should()
            .Be(value)
            ;
    }
}
