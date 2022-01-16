namespace DietMenu.Core.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DietMenu.Core.Application.Common;
using DietMenu.Core.Infrastructure.HealthChecks.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if DEBUG
[TestClass]
public sealed class StartupTests
{
    private readonly WebApplicationFactory<Program> factory;

    public StartupTests()
    {
        this.factory = new WebApplicationFactory<Program>();
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }

    [TestMethod]
    public async Task ShouldReturnHealthCheck()
    {
        //GIVEN
        var client = this.factory.CreateClient();
        const string url = "/health";

        //WHEN
        var response = await client.GetAsync(url);

        //THEN
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK)
            ;
        response.Content.Headers.ContentType?.ToString()
            .Should()
            .Be("application/json; charset=utf-8")
            ;

        var baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<HealthCheck>>();
        baseResponse.Should()
            .NotBeNull()
            ;
        baseResponse!.Code.Should()
            .Be("OK")
            ;
        baseResponse.Error.Should()
            .BeNull()
            ;
        baseResponse.Response.Should()
            .NotBeNull()
            ;
        baseResponse.Response!.HealthChecks.Count.Should()
            .Be(2)
            ;

        baseResponse.Response.HealthChecks.Should()
            .ContainSingle(i => i.Component == "Identity")
            ;
        baseResponse.Response.HealthChecks.Should()
            .ContainSingle(i => i.Component == "SQL Default")
            ;
    }

    [TestMethod]
    public async Task ShouldReturnSwagger()
    {
        //GIVEN
        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        //THEN
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK)
            ;

        response.Content.Headers.ContentType?.ToString()
            .Should()
            .Be("application/json; charset=utf-8")
            ;
    }
}
#endif
