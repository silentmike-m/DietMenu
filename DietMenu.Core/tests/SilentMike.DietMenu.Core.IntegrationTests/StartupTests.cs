namespace SilentMike.DietMenu.Core.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Infrastructure.HealthChecks.Models;

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
            .Be(4)
            ;

        baseResponse.Response.HealthChecks.Should()
            .ContainSingle(i => i.Component == "Db Context")
            ;
        baseResponse.Response.HealthChecks.Should()
            .ContainSingle(i => i.Component == "RabbitMQ")
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
