namespace SilentMike.DietMenu.Auth.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Infrastructure.HealthCheck.Models;
using SilentMike.DietMenu.Auth.IntegrationTests.Helpers;

[TestClass]
public sealed class StartupTests : IDisposable
{
    private const string HEALTH_CHECK_URL = "/health";
    private const string SWAGGER_URL = "/swagger/v1/swagger.json";

    private readonly WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>()
        .WithFakeDbContext();

    public void Dispose()
    {
        this.factory.Dispose();
    }

    [TestMethod]
    public async Task Should_Return_Health_Check()
    {
        //GIVEN
        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.GetAsync(HEALTH_CHECK_URL);

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
            .Be(4)
            ;

        healthCheckResponse.HealthChecks.Should()
            .ContainSingle(i => i.Component == "Identity")
            ;

        healthCheckResponse.HealthChecks.Should()
            .ContainSingle(i => i.Component == "masstransit-bus")
            ;

        healthCheckResponse.HealthChecks.Should()
            .ContainSingle(i => i.Component == "RabbitMQ")
            ;

        healthCheckResponse.HealthChecks.Should()
            .ContainSingle(i => i.Component == "SQL Identity")
            ;
    }

    [TestMethod]
    public async Task Should_Return_Swagger()
    {
        //GIVEN
        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.GetAsync(SWAGGER_URL);

        //THEN
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK)
            ;

        response.Content.Headers.ContentType?.ToString()
            .Should()
            .Be("text/html; charset=utf-8")
            ;
    }
}
