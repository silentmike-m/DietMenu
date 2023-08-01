namespace SilentMike.DietMenu.Auth.IntegrationTests.Services;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.IntegrationTests.Helpers;
using SilentMike.DietMenu.Auth.Web.Common.Constants;

[TestClass]
public sealed class IdentityPageUrlServiceTests : IDisposable
{
    private const string DEFAULT_CLIENT_URI = "https://client.domain.com";
    private const string ISSUER_URI = "https://test.domain.com";

    private readonly WebApplicationFactory<Program> factory;

    public IdentityPageUrlServiceTests()
        => this.factory = new WebApplicationFactory<Program>()
            .WithFakeDbContext();

    public void Dispose()
    {
        this.factory.Dispose();
    }

    [TestMethod]
    public void Should_Build_Confirm_User_Email_Page_Url_On_Get_Confirm_User_Email_Page_Url()
    {
        //GIVEN
        var hostUri = new Uri(ISSUER_URI);
        var returnUri = new Uri(DEFAULT_CLIENT_URI);
        const string token = "confirm_token";
        var userId = Guid.NewGuid();

        var service = this.factory.Services.GetRequiredService<IIdentityPageUrlService>();

        var expectedUrl = $@"{ISSUER_URI}/{IdentityPageNames.AREA}{IdentityPageNames.CONFIRM_USER_EMAIL}?ReturnUrl={returnUri.Scheme}%3A%2F%2F{returnUri.Host}%2F&Token={token}&UserId={userId}";

        //WHEN
        var resultUrl = service.GetConfirmUserEmailPageUrl(hostUri, returnUri, token, userId);

        //THEN
        expectedUrl.Should()
            .Be(resultUrl)
            ;
    }
}
