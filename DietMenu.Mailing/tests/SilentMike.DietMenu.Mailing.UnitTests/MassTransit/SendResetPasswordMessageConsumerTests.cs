namespace SilentMike.DietMenu.Mailing.UnitTests.MassTransit;

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Shared.MassTransit;

[TestClass]
public sealed class SendResetPasswordMessageConsumerTests
{
    private const string EMAIL = "user@domain.com";
    private const string URL = "login_url";

    [TestMethod]
    public async Task ShouldSendCreatedUserEmail()
    {
        SendResetPasswordEmail? command = null;

        //GIVEN
        var logger = new NullLogger<SendResetPasswordMessageConsumer>();

        var mediator = new Mock<IMediator>();
        mediator.Setup(i => i.Send(It.IsAny<SendResetPasswordEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => command = request as SendResetPasswordEmail);

        var message = new Mock<ISendResetPasswordMessage>();
        message.Setup(i => i.Email)
            .Returns(EMAIL)
            ;
        message.Setup(i => i.Url)
            .Returns(URL)
            ;

        var context = new Mock<ConsumeContext<ISendResetPasswordMessage>>();
        context.Setup(i => i.Message)
            .Returns(message.Object);

        var consumer = new SendResetPasswordMessageConsumer(logger, mediator.Object);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        command.Should()
            .NotBeNull()
            ;
        command!.Email.Should()
            .Be(EMAIL)
            ;
        command.Url.Should()
            .Be(URL)
            ;
    }
}
