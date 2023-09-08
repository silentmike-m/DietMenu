namespace SilentMike.DietMenu.Mailing.UnitTests;

using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Services;

[TestClass]
public sealed class XmlServiceUnitTests
{
    [TestMethod]
    public async Task Should_Throw_Resource_Not_Found_When_Invalid_Resource_Name()
    {
        //GIVEN
        const string resourceName = "Test";
        var service = new XmlService();

        //WHEN
        var action = async () => await service.GetXsltStringAsync(resourceName, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowExactlyAsync<ResourceNotFoundException>()
                .WithMessage($"*{resourceName}*")
            ;
    }
}
