namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit;

using System;
using System.Threading.Tasks;
using FluentAssertions;
using global::MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Extensions;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Auth.UnitTests.Services;
using SilentMike.DietMenu.Shared.Email.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;
using SilentMike.DietMenu.Shared.Identity.Models;

[TestClass]
public sealed class IdentityDataRequestConsumerTests
{
    private readonly string systemUserEmail = "system@domain.com";
    private readonly string userEmail = "test@domain.com";
    private readonly Guid userId = Guid.NewGuid();

    private readonly Mock<ConsumeContext<IIdentityDataRequest>> context = new();
    private readonly IOptions<IdentityOptions> identityOptions;
    private readonly NullLogger<IdentityDataRequestConsumer> logger = new();
    private readonly Mock<FakeUserManager> userManager;

    public IdentityDataRequestConsumerTests()
    {
        var user = new DietMenuUser
        {
            Id = this.userId.ToString(),
            Email = this.userEmail,
        };

        this.userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(this.userId.ToString()))
                .Returns(Task.FromResult(user)))
            .Build();

        var options = new IdentityOptions
        {
            SystemUserEmail = this.systemUserEmail,
        };

        this.identityOptions = Options.Create(options);
    }

    [TestMethod]
    public async Task ShouldThrowFormatExceptionWhenInvalidPayloadType()
    {
        //GIVEN
        var message = Mock.Of<IIdentityDataRequest>(dataMessage => dataMessage.PayloadType == typeof(string).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        var consumer = new IdentityDataRequestConsumer(this.identityOptions, this.logger, userManager.Object);

        //WHEN
        Func<Task> action = async () => await consumer.Consume(this.context.Object);

        //THEN
        await action.Should()
                .ThrowAsync<FormatException>()
            ;
    }

    [TestMethod]
    public async Task ShouldThrowUserNotFoundWhenInvalidFamilyIdOnIdentityDataRequest()
    {
        //GIVEN
        var payload = new GetFamilyUserEmailPayload
        {
            FamilyId = Guid.NewGuid(),
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IIdentityDataRequest>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(GetFamilyUserEmailPayload).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        var consumer = new IdentityDataRequestConsumer(this.identityOptions, this.logger, userManager.Object);

        //WHEN
        Func<Task> action = async () => await consumer.Consume(this.context.Object);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task ShouldRespondWithFamilyUserEmailOnIdentityDataRequest()
    {
        GetFamilyUserEmailResponse? getFamilyUserEmailResponse = null;

        //GIVEN
        var payload = new GetFamilyUserEmailPayload
        {
            FamilyId = this.userId,
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IIdentityDataRequest>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(GetFamilyUserEmailPayload).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        context.Setup(i => i.RespondAsync(It.IsAny<GetFamilyUserEmailResponse>()))
            .Callback<GetFamilyUserEmailResponse>((response) => getFamilyUserEmailResponse = response);

        var consumer = new IdentityDataRequestConsumer(this.identityOptions, this.logger, userManager.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        getFamilyUserEmailResponse.Should()
            .NotBeNull()
            ;
        getFamilyUserEmailResponse!.Email.Should()
            .Be(this.userEmail)
            ;
    }

    [TestMethod]
    public async Task ShouldRespondWithSystemUserEmailOnIdentityDataRequest()
    {
        GetSystemUserEmailResponse? getSystemUserEmailResponse = null;

        //GIVEN
        var payload = new GetSystemUserEmailPayload();

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IIdentityDataRequest>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(VerifyUserEmailPayload).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        context.Setup(i => i.RespondAsync(It.IsAny<GetSystemUserEmailResponse>()))
            .Callback<GetSystemUserEmailResponse>((response) => getSystemUserEmailResponse = response);

        var consumer = new IdentityDataRequestConsumer(this.identityOptions, this.logger, userManager.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        getSystemUserEmailResponse.Should()
            .NotBeNull()
            ;
        getSystemUserEmailResponse!.Email.Should()
            .Be(this.systemUserEmail)
            ;
    }
}
