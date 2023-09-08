namespace SilentMike.DietMenu.Auth.UnitTests;

using System.Reflection;

[TestClass]
public sealed class MediatRTests
{
    private const string APPLICATION_ASSEMBLY_NAME = "SilentMike.DietMenu.Auth.Application";
    private const string INFRASTRUCTURE_ASSEMBLY_NAME = "SilentMike.DietMenu.Auth.Infrastructure";

    private static readonly Type NOTIFICATION_HANDLER_TYPE = typeof(INotificationHandler<>);
    private static readonly Type NOTIFICATION_TYPE = typeof(INotification);

    private static readonly List<Type> REQUEST_HANDLER_TYPES = new()
    {
        typeof(IRequestHandler<,>),
        typeof(IRequestHandler<>),
    };

    private static readonly List<Type> REQUEST_TYPES = new()
    {
        typeof(IRequest<>),
        typeof(IRequest<object?>),
    };

    [TestMethod]
    public void Should_Contain_Handler_For_All_Notifications()
    {
        //GIVEN
        var types = Assembly.Load(new AssemblyName(APPLICATION_ASSEMBLY_NAME)).GetTypes().ToList();
        types.AddRange(Assembly.Load(new AssemblyName(INFRASTRUCTURE_ASSEMBLY_NAME)).GetTypes());

        var notificationTypes = types
            .Where(type => NOTIFICATION_TYPE.IsAssignableFrom(type))
            .ToList();

        //WHEN
        var handlerTypes = types
            .Where(type => type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == NOTIFICATION_HANDLER_TYPE))
            .ToList();

        //THEN
        var errors = new List<string>();

        foreach (var notificationType in notificationTypes)
        {
            var existsHandler = handlerTypes
                .Any(handler => handler.GetInterfaces()
                    .Any(type => type.GenericTypeArguments
                        .Any(argument => argument == notificationType)));

            if (existsHandler is false)
            {
                errors.Add($"Missing handler for notification {notificationType}");
            }
        }

        errors.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public void Should_Contain_Single_Handler_For_All_Requests_And_Queries()
    {
        //GIVEN
        var types = Assembly.Load(new AssemblyName(APPLICATION_ASSEMBLY_NAME)).GetTypes().ToList();
        types.AddRange(Assembly.Load(new AssemblyName(INFRASTRUCTURE_ASSEMBLY_NAME)).GetTypes());

        var requestTypes = new List<Type>();

        foreach (var type in types)
        {
            var isRequestType = REQUEST_TYPES.Any(requestType => requestType.IsAssignableFrom(type));

            if (isRequestType)
            {
                requestTypes.Add(type);
            }
        }

        //WHEN
        var handlerTypes = new List<Type>();

        foreach (var type in types)
        {
            var handlerInterface = type.GetInterfaces()
                .Where(interfaceType => interfaceType.IsGenericType)
                .SingleOrDefault(interfaceType => REQUEST_HANDLER_TYPES.Contains(interfaceType.GetGenericTypeDefinition()));

            if (handlerInterface is not null)
            {
                handlerTypes.Add(type);
            }
        }

        //THEN
        foreach (var requestType in requestTypes)
        {
            handlerTypes.Should().ContainSingle(type =>
                    type.GetInterfaces().Any(j => j.GenericTypeArguments.Any(ta => ta == requestType)), $"Missing handler for request {requestType}");
        }
    }
}
