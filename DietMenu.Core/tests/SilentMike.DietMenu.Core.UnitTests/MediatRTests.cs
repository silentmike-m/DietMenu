namespace SilentMike.DietMenu.Core.UnitTests;

using System.Linq;
using System.Reflection;
using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class MediatRTests
{
    [TestMethod]
    public void ShouldContainSingleHandlerForAllRequestsAndQueries()
    {
        //GIVEN
        var types = Assembly.Load(new AssemblyName("SilentMike.DietMenu.Core.Application")).GetTypes().ToList();
        types.AddRange(Assembly.Load(new AssemblyName("SilentMike.DietMenu.Core.Infrastructure")).GetTypes());

        var requestTypes = types.Where(i => typeof(IRequest).IsAssignableFrom(i) || typeof(IRequest<object?>).IsAssignableFrom(i)).ToList();
        var handlerTypes = types.Where(i =>
            i.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))).ToList();

        //THEN
        foreach (var requestType in requestTypes)
        {
            handlerTypes.Should().ContainSingle(i =>
                i.GetInterfaces().Any(j => j.GenericTypeArguments.Any(ta => ta == requestType)), $"Missing handler for request {requestType}");
        }
    }

    [TestMethod]
    public void ShouldContainHandlerForAllNotifications()
    {
        //GIVEN
        var types = Assembly.Load(new AssemblyName("SilentMike.DietMenu.Core.Application")).GetTypes().ToList();
        types.AddRange(Assembly.Load(new AssemblyName("SilentMike.DietMenu.Core.Infrastructure")).GetTypes());

        var notificationTypes = types.Where(i => typeof(INotification).IsAssignableFrom(i)).ToList();
        var handlerTypes = types.Where(i =>
            i.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>))).ToList();

        //THEN
        foreach (var notificationType in notificationTypes)
        {
            handlerTypes.Should().Contain(i =>
                i.GetInterfaces().Any(j => j.GenericTypeArguments.Any(ta => ta == notificationType)), $"Missing handler for notification {notificationType}");
        }
    }
}
