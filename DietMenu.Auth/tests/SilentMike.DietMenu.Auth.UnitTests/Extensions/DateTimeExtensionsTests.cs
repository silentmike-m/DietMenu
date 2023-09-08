namespace SilentMike.DietMenu.Auth.UnitTests.Extensions;

using SilentMike.DietMenu.Auth.Application.Common.Extensions;

[TestClass]
public sealed class DateTimeExtensionsTests
{
    private static readonly DateTime DATE = new(year: 2023, month: 8, day: 1, hour: 11, minute: 47, second: 15, millisecond: 16, DateTimeKind.Utc);

    [TestMethod]
    public void Should_Trim_Date_Time()
    {
        //GIVEN

        //WHEN
        var result = DATE.ToTrimmedDateTime();

        //THEN
        var expectedResult = new DateTime(DATE.Year, DATE.Month, DATE.Day, DATE.Hour, DATE.Minute, DATE.Second, DATE.Kind);

        result.Should()
            .Be(expectedResult)
            ;
    }
}
