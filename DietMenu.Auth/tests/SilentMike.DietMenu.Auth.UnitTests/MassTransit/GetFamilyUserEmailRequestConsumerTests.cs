namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit;

using System;
using System.Threading.Tasks;
using FluentAssertions;
using global::MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Auth.UnitTests.Services;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

[TestClass]
public sealed class GetFamilyUserEmailRequestConsumerTests
{
    [TestMethod]
    public async Task ShouldThrowUserNotFoundWhenInvalidIdOnGetFamilyUserEmailRequestConsumer()
    {
        //GIVEN
        var message = Mock.Of<IGetFamilyUserEmailRequest>();

        var context = new Mock<ConsumeContext<IGetFamilyUserEmailRequest>>();

        context.Setup(i => i.Message)
            .Returns(message);

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .Build();

        var logger = new NullLogger<GetFamilyUserEmailRequestConsumer>();

        var consumer = new GetFamilyUserEmailRequestConsumer(logger, userManager.Object);

        //WHEN
        Func<Task> action = async () => await consumer.Consume(context.Object);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(i => i.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnFamilyUserEmail()
    {
        IGetFamilyUserEmailResponse? getFamilyUserEmailResponse = null;

        //GIVEN
        var user = new DietMenuUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "user@domain.com",
        };

        var message = Mock.Of<IGetFamilyUserEmailRequest>();

        var context = new Mock<ConsumeContext<IGetFamilyUserEmailRequest>>();

        context.Setup(i => i.Message)
            .Returns(message);

        context.Setup(i => i.RespondAsync(It.IsAny<IGetFamilyUserEmailResponse>()))
            .Callback<IGetFamilyUserEmailResponse>((response) => getFamilyUserEmailResponse = response);

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user)))
            .Build();

        var logger = new NullLogger<GetFamilyUserEmailRequestConsumer>();

        var consumer = new GetFamilyUserEmailRequestConsumer(logger, userManager.Object);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        getFamilyUserEmailResponse.Should()
            .NotBeNull()
            ;
        getFamilyUserEmailResponse!.Email.Should()
            .Be(user.Email)
            ;
    }
}
