namespace SilentMike.DietMenu.Mailing.UnitTests.Smtp.CommandHandlers;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Models;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp.CommandHandlers;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp.Interfaces;

[TestClass]
public sealed class SendEmailHandlerTests
{
    private const string FROM = "receiver@domain.com";

    private readonly NullLogger<SendEmailHandler> logger = new();
    private readonly Mock<IMailService> mailService = new();
    private readonly IOptions<SmtpOptions> options;

    public SendEmailHandlerTests()
        => this.options = Options.Create(new SmtpOptions
        {
            From = FROM,
        });

    [TestMethod]
    public async Task Should_Send_Email()
    {
        //GIVEN
        MimeMessage? sentMessage = null;

        this.mailService
            .Setup(service => service.SendEmailAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()))
            .Callback<MimeMessage, CancellationToken>((message, _) => sentMessage = message);

        var request = new SendEmail
        (
            new Email
            (
                "html message",
                new List<EmailLinkedResource>(),
                "receiver@domain.com", "subject",
                "text message"
            )
        );

        var handler = new SendEmailHandler(this.logger, this.mailService.Object, this.options);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.mailService.Verify(service => service.SendEmailAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        sentMessage.Should()
            .NotBeNull()
            ;

        sentMessage!.From.Count.Should()
            .Be(1)
            ;

        var fromAddress = MailboxAddress.Parse(ParserOptions.Default, FROM);

        sentMessage!.From[0].Should()
            .BeEquivalentTo(fromAddress)
            ;

        sentMessage.To.Count.Should()
            .Be(1)
            ;

        var receiverAddress = MailboxAddress.Parse(ParserOptions.Default, request.Email.Receiver);

        sentMessage.To[0].Should()
            .BeEquivalentTo(receiverAddress)
            ;

        sentMessage.Subject.Should()
            .Be(request.Email.Subject)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Operation_Cancelled_When_Cancellation_Token_Is_Cancelled()
    {
        //GIVEN
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var request = new SendEmail
        (
            new Email
            (
                "html message",
                new List<EmailLinkedResource>(),
                "receiver@domain.com", "subject",
                "text message"
            )
        );

        var handler = new SendEmailHandler(this.logger, this.mailService.Object, this.options);

        //WHEN
        cancellationTokenSource.Cancel();

        var action = async () => await handler.Handle(request, cancellationToken);

        //THEN
        await action.Should()
                .ThrowAsync<OperationCanceledException>()
            ;
    }
}
