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
public sealed class SendCreatedUserMessageConsumerTests
{
    private const string EMAIL = "test@test.pl";
    private const string URL = "login_url";
    private const string USER_NAME = "user_name";

    [TestMethod]
    public async Task ShouldSendCreatedUserEmail()
    {
        SendVerifyEmail? command = null;

        //GIVEN
        var logger = new NullLogger<SendVerifyEmailRequestConsumer>();

        var mediator = new Mock<IMediator>();
        mediator.Setup(i => i.Send(It.IsAny<SendVerifyEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => command = request as SendVerifyEmail);

        var message = new Mock<ISendVerifyEmailRequest>();
        message.Setup(i => i.Email)
            .Returns(EMAIL)
            ;
        message.Setup(i => i.Url)
            .Returns(URL)
            ;
        message.Setup(i => i.UserName)
            .Returns(USER_NAME)
            ;

        var context = new Mock<ConsumeContext<ISendVerifyEmailRequest>>();
        context.Setup(i => i.Message)
            .Returns(message.Object);

        var consumer = new SendVerifyEmailRequestConsumer(logger, mediator.Object);

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
        command.UserName.Should()
            .Be(USER_NAME)
            ;
    }
}