namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit;

using System.Threading.Tasks;
using FluentAssertions;
using global::MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

[TestClass]
public sealed class GetSystemUserEmailRequestConsumerTests
{
    [TestMethod]
    public async Task ShouldReturnSystemUserEmail()
    {
        IGetSystemUserEmailResponse? getSystemUserEmailResponse = null;

        //GIVEN
        var message = Mock.Of<IGetSystemUserEmailRequest>();

        var context = new Mock<ConsumeContext<IGetSystemUserEmailRequest>>();

        context.Setup(i => i.Message)
            .Returns(message);

        context.Setup(i => i.RespondAsync(It.IsAny<IGetSystemUserEmailResponse>()))
            .Callback<IGetSystemUserEmailResponse>((response) => getSystemUserEmailResponse = response);

        var identityOptions = new IdentityOptions
        {
            SystemUserEmail = "system@domain.com",
        };

        var options = Options.Create(identityOptions);

        var logger = new NullLogger<GetSystemUserEmailRequestConsumer>();

        var consumer = new GetSystemUserEmailRequestConsumer(options, logger);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        getSystemUserEmailResponse.Should()
            .NotBeNull()
            ;
        getSystemUserEmailResponse!.Email.Should()
            .Be(identityOptions.SystemUserEmail)
            ;
    }
}
