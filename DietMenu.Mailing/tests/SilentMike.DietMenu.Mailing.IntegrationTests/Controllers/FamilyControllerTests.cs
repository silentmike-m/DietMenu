namespace SilentMike.DietMenu.Mailing.IntegrationTests.Controllers;

using System.Net.Http.Json;
using System.Reflection;
using FluentAssertions;
using MassTransit;
using MassTransit.Clients;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using netDumbster.smtp;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Family.CommandHandlers;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

[TestClass]
public sealed class FamilyControllerTests
{
    private const string FAMILY_ID = "b99d225c-e236-4f83-8c70-964112799ff4";
    private const string FAMILY_USER_EMAIL = "family@domain.com";
    private const string URL = "Family/SendImportedFamilyDataEmail";

    private static readonly SmtpOptions SMTP_OPTIONS = new()
    {
        From = "from@domain.com",
        Host = "localhost",
        Port = 9009,
    };

    private readonly WebApplicationFactory<Program> factory;
    private readonly Mock<IRequestClient<IGetFamilyUserEmailRequest>> requestClient = new();

    public FamilyControllerTests()
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
                        .AddScoped<IRequestHandler<SendImportedFamilyDataEmail>, SendImportedFamilyDataEmailHandler>();

                    services.AddMassTransitTestHarness();

                    services.AddScoped(_ => this.requestClient.Object);
                });
            });
    }

    [TestMethod]
    public async Task Should_Send_Imported_Family_Data_Email()
    {
        //GIVEN
        var message = Mock.Of<IGetFamilyUserEmailResponse>(message => message.Email == FAMILY_USER_EMAIL);

        var context = new Mock<ConsumeContext<IGetFamilyUserEmailResponse>>();

        context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        this.requestClient
            .Setup(service => service.GetResponse<IGetFamilyUserEmailResponse>(It.IsAny<IGetFamilyUserEmailRequest>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
            .ReturnsAsync((IGetFamilyUserEmailRequest message2, CancellationToken _, RequestTimeout _) =>
            {
                if (message2.FamilyId == new Guid(FAMILY_ID))
                {
                    return new MessageResponse<IGetFamilyUserEmailResponse>(context.Object);
                }

                return null!;
            });

        var request = new SendImportedFamilyDataEmail
        {
            FamilyId = new Guid(FAMILY_ID),
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
            .And
            .Contain(part => part.HeaderData == "text/plain; charset=utf-8" && !string.IsNullOrEmpty(part.BodyData))
            ;

        email.Subject.Should()
            .Be(EmailSubjects.IMPORTED_FAMILY_DATA_EMAIL_SUBJECT)
            ;

        email.ToAddresses.Should()
            .HaveCount(1)
            ;

        email.ToAddresses[0].Address.Should()
            .Be(FAMILY_USER_EMAIL)
            ;
    }
}
