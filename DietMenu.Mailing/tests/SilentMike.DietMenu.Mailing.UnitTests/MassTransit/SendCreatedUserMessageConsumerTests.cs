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
    private const string FAMILY_NAME = "family_name";
    private const string LOGIN_URL = "login_url";
    private const string USER_NAME = "user_name";

    [TestMethod]
    public async Task ShouldSendCreatedUserEmail()
    {
        SendCreatedUserEmail? command = null;

        //GIVEN
        var logger = new NullLogger<SendCreatedUserMessageConsumer>();

        var mediator = new Mock<IMediator>();
        mediator.Setup(i => i.Send(It.IsAny<SendCreatedUserEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => command = request as SendCreatedUserEmail);

        var message = new Mock<ISendCreatedUserMessage>();
        message.Setup(i => i.Email)
            .Returns(EMAIL)
            ;
        message.Setup(i => i.FamilyName)
            .Returns(FAMILY_NAME)
            ;
        message.Setup(i => i.LoginUrl)
            .Returns(LOGIN_URL)
            ;
        message.Setup(i => i.UserName)
            .Returns(USER_NAME)
            ;

        var context = new Mock<ConsumeContext<ISendCreatedUserMessage>>();
        context.Setup(i => i.Message)
            .Returns(message.Object);

        var consumer = new SendCreatedUserMessageConsumer(logger, mediator.Object);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        command.Should()
            .NotBeNull()
            ;
        command!.Email.Should()
            .Be(EMAIL)
            ;
        command.FamilyName.Should()
            .Be(FAMILY_NAME)
            ;
        command.LoginUrl.Should()
            .Be(LOGIN_URL)
            ;
        command.UserName.Should()
            .Be(USER_NAME)
            ;
    }
}