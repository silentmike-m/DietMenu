namespace SilentMike.DietMenu.Mailing.UnitTests.MassTransit;

using FluentAssertions;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;
using SilentMike.DietMenu.Mailing.Application.Family.Models;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Infrastructure.Extensions;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Shared.Email.Interfaces;
using SilentMike.DietMenu.Shared.Email.Models;

[TestClass]
public sealed class EmailDataMessageConsumerTests
{
    private readonly Mock<ConsumeContext<IEmailDataMessage>> context = new();
    private readonly NullLogger<EmailDataMessageConsumer> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Send_Reset_Password_Email_Request()
    {
        SendResetPasswordEmail? sendResetPasswordEmailRequest = null;

        //GIVEN
        this.mediator
            .Setup(service => service.Send(It.IsAny<SendResetPasswordEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest, CancellationToken>((request, _) => sendResetPasswordEmailRequest = request as SendResetPasswordEmail);

        var payload = new ResetUserPasswordEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IEmailDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(ResetUserPasswordEmailPayload).FullName);

        this.context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<SendResetPasswordEmail>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new SendResetPasswordEmail
        {
            Email = payload.Email,
            Url = payload.Url,
        };

        sendResetPasswordEmailRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }

    [TestMethod]
    public async Task Should_Send_Verify_User_Email_Request()
    {
        SendVerifyUserEmail? sendVerifyUserEmail = null;

        //GIVEN
        this.mediator
            .Setup(service => service.Send(It.IsAny<SendVerifyUserEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest, CancellationToken>((request, _) => sendVerifyUserEmail = request as SendVerifyUserEmail);

        var payload = new ConfirmUserEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IEmailDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(ConfirmUserEmailPayload).FullName);

        this.context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<SendVerifyUserEmail>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new SendResetPasswordEmail
        {
            Email = payload.Email,
            Url = payload.Url,
        };

        sendVerifyUserEmail.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }

    [TestMethod]
    public async Task Should_Sent_Send_Imported_Family_Data_Email()
    {
        //GIVEN
        SendImportedFamilyDataEmail? sendImportedFamilyDataEmail = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<SendImportedFamilyDataEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest, CancellationToken>((request, _) => sendImportedFamilyDataEmail = request as SendImportedFamilyDataEmail);

        var payloadResultError = new ImportFamilyDataError("error code", "error message");

        var payloadResult = new ImportFamilyDataResult
        {
            DataArea = "ingredients",
            Errors = new List<ImportFamilyDataError>
            {
                payloadResultError,
            },
        };

        var payload = new ImportedFamilyDataPayload
        {
            ErrorCode = null,
            ErrorMessage = null,
            FamilyId = Guid.NewGuid(),
            Results = new List<ImportFamilyDataResult>
            {
                payloadResult,
            },
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IEmailDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(ImportedFamilyDataPayload).FullName);

        this.context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<SendImportedFamilyDataEmail>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new SendImportedFamilyDataEmail
        {
            ErrorCode = payload.ErrorCode,
            ErrorMessage = payload.ErrorMessage,
            FamilyId = payload.FamilyId,
            IsSuccess = false,
            Results = new List<ImportedFamilyDataResult>
            {
                new()
                {
                    DataArea = payloadResult.DataArea,
                    Errors = new List<ImportedFamilyDataError>
                    {
                        new()
                        {
                            Code = payloadResultError.Code,
                            Message = payloadResultError.Message,
                        },
                    },
                },
            },
        };

        sendImportedFamilyDataEmail.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Format_Exception_When_Invalid_Payload_Type()
    {
        //GIVEN
        var message = Mock.Of<IEmailDataMessage>(dataMessage => dataMessage.PayloadType == typeof(string).FullName);

        this.context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        var action = async () => await consumer.Consume(this.context.Object);

        //THEN
        await action.Should()
                .ThrowAsync<FormatException>()
            ;
    }
}
