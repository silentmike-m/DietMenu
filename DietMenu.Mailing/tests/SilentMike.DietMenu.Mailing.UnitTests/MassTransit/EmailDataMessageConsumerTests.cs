namespace SilentMike.DietMenu.Mailing.UnitTests.MassTransit;

using global::MassTransit;
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
    private readonly ConsumeContext<IEmailDataMessage> context = Substitute.For<ConsumeContext<IEmailDataMessage>>();
    private readonly NullLogger<EmailDataMessageConsumer> logger = new();
    private readonly ISender mediator = Substitute.For<ISender>();

    [TestMethod]
    public async Task Should_Send_Reset_Password_Email_Request()
    {
        //GIVEN
        SendResetPasswordEmail? sendResetPasswordEmailRequest = null;

        await this.mediator
            .Send(Arg.Do<SendResetPasswordEmail>(request => sendResetPasswordEmailRequest = request), Arg.Any<CancellationToken>());

        var payload = new ResetUserPasswordEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Substitute.For<IEmailDataMessage>();
        message.Payload.Returns(payloadJson);
        message.PayloadType.Returns(typeof(ResetUserPasswordEmailPayload).FullName);

        this.context
            .Message
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator);

        //WHEN
        await consumer.Consume(this.context);

        //THEN
        _ = this.mediator.Received(1).Send(Arg.Any<SendResetPasswordEmail>(), Arg.Any<CancellationToken>());

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
        //GIVEN
        SendVerifyUserEmail? sendVerifyUserEmail = null;

        await this.mediator
            .Send(Arg.Do<SendVerifyUserEmail>(request => sendVerifyUserEmail = request), Arg.Any<CancellationToken>());

        var payload = new ConfirmUserEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Substitute.For<IEmailDataMessage>();
        message.Payload.Returns(payloadJson);
        message.PayloadType.Returns(typeof(ConfirmUserEmailPayload).FullName);

        this.context
            .Message
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator);

        //WHEN
        await consumer.Consume(this.context);

        //THEN
        _ = this.mediator.Received(1).Send(Arg.Any<SendVerifyUserEmail>(), Arg.Any<CancellationToken>());

        var expectedRequest = new SendVerifyUserEmail
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

        await this.mediator
            .Send(Arg.Do<SendImportedFamilyDataEmail>(request => sendImportedFamilyDataEmail = request), Arg.Any<CancellationToken>());

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

        var message = Substitute.For<IEmailDataMessage>();
        message.Payload.Returns(payloadJson);
        message.PayloadType.Returns(typeof(ImportedFamilyDataPayload).FullName);

        this.context
            .Message
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator);

        //WHEN
        await consumer.Consume(this.context);

        //THEN
        _ = this.mediator.Received(1).Send(Arg.Any<SendImportedFamilyDataEmail>(), Arg.Any<CancellationToken>());

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
        var message = Substitute.For<IEmailDataMessage>();
        message.PayloadType.Returns(typeof(string).FullName);

        this.context
            .Message
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator);

        //WHEN
        var action = async () => await consumer.Consume(this.context);

        //THEN
        await action.Should()
                .ThrowAsync<FormatException>()
            ;
    }
}
