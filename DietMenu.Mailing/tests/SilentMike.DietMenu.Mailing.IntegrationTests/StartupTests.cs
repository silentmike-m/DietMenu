namespace SilentMike.DietMenu.Mailing.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Mailing.Infrastructure.HealthChecks.Models;

#if DEBUG
[TestClass]
public sealed class StartupTests
{
    private readonly WebApplicationFactory<Program> factory;

    public StartupTests()
        => this.factory = new WebApplicationFactory<Program>();

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
            .Be("application/json")
            ;

        var healthCheckResponse = await response.Content.ReadFromJsonAsync<HealthCheck>();

        healthCheckResponse.Should()
            .NotBeNull()
            ;

        healthCheckResponse!.HealthChecks.Count.Should()
            .Be(2)
            ;

        healthCheckResponse.HealthChecks.Should()
            .ContainSingle(i => i.Component == "RabbitMQ")
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
