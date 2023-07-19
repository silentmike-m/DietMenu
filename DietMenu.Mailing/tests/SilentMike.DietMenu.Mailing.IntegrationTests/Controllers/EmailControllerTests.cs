namespace SilentMike.DietMenu.Mailing.IntegrationTests.Controllers;

using System.Net.Http.Json;
using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netDumbster.smtp;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Models;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp.CommandHandlers;

[TestClass]
public sealed class EmailControllerTests
{
    private const string URL = "Email/SendEmail";

    private static readonly SmtpOptions SMTP_OPTIONS = new()
    {
        From = "from@domain.com",
        Host = "localhost",
        Port = 9009,
    };

    private readonly WebApplicationFactory<Program> factory;

    public EmailControllerTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Smtp:From", SMTP_OPTIONS.From },
            { "Smtp:Port", SMTP_OPTIONS.Port.ToString() },
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        this.factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseConfiguration(configuration);

                builder.ConfigureTestServices(services =>
                {
                    services.AddMediatR(config =>
                        {
                            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                        })
                        .AddScoped<IRequestHandler<SendEmail>, SendEmailHandler>();
                });
            });
    }

    [TestMethod]
    public async Task Should_Send_Email()
    {
        //GIVEN
        var request = new SendEmailRequest
        {
            Receiver = "receiver@domain.com",
            Subject = "subject",
            Text = "email text",
        };

        var client = this.factory.CreateClient();

        using var smtpServer = SimpleSmtpServer.Start(SMTP_OPTIONS.Port);

        //WHEN
        var response = await client.PostAsJsonAsync(URL, request);

        //THEN
        response.EnsureSuccessStatusCode();

        smtpServer.ReceivedEmail.Should()
            .HaveCount(1)
            ;

        var email = smtpServer.ReceivedEmail[0];

        email.FromAddress.Address.Should()
            .Be(SMTP_OPTIONS.From)
            ;

        email.MessageParts.Should()
            .HaveCount(1)
            ;

        email.MessageParts[0].HeaderData.Should()
            .Be("text/plain; charset=utf-8")
            ;

        email.MessageParts[0].BodyData.Should()
            .Be(request.Text)
            ;

        email.Subject.Should()
            .Be(request.Subject)
            ;

        email.ToAddresses.Should()
            .HaveCount(1)
            ;

        email.ToAddresses[0].Address.Should()
            .Be(request.Receiver)
            ;
    }
}
