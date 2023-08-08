namespace SilentMike.DietMenu.Core.InfrastructureTests.Extensions;

using SilentMike.DietMenu.Core.Infrastructure.Common.Extensions;

[TestClass]
public sealed class UriBuilderExtensionsTests
{
    private const string URL = "http://www.domain.com";

    [TestMethod]
    public void Should_Return_Uri_With_Path()
    {
        //GIVEN
        var uriBuilder = new UriBuilder(new Uri(URL));
        const string path = "get/token";

        //WHEN
        var result = uriBuilder.GetUriWithPath(path);

        //THEN
        result.Should()
            .Be($"{URL}/{path}")
            ;
    }
}
