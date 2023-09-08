namespace SilentMike.DietMenu.Mailing.UnitTests.Extensions;

using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.Application.Extensions;

[TestClass]
public sealed record ResourceExtensionsTests
{
    [TestMethod]
    public async Task Should_Return_Resource_Bytes_On_Get_Resource_Bytes()
    {
        //GIVEN
        const string resourceName = "SilentMike.DietMenu.Mailing.Application.Resources.Family.ImportedFamilyDataTextEmail.xslt";

        //WHEN
        var result = await resourceName.GetResourceBytesAsync();

        //tHEN
        result.Should()
            .NotBeNullOrEmpty()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Resource_Not_Found_When_Missing_Resource_On_Get_Resource_Bytes()
    {
        //GIVEN
        const string invalidResourceName = "test";

        //WHEN
        var action = async () => await invalidResourceName.GetResourceBytesAsync();

        //THEN
        await action.Should()
                .ThrowExactlyAsync<ResourceNotFoundException>()
                .WithMessage($"*{invalidResourceName}*")
            ;
    }
}
