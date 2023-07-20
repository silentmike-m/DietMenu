namespace SilentMike.DietMenu.Auth.Application.Common.Behaviors;

using SilentMike.DietMenu.Auth.Application.Exceptions;
using SilentMike.DietMenu.Auth.Domain.Enums;

internal sealed class SystemBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, ISystemRequest
{
    private readonly ICurrentRequestService currentUserService;

    public SystemBehavior(ICurrentRequestService currentUserService)
        => this.currentUserService = currentUserService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var role = this.currentUserService.CurrentUserRole;

        var systemRole = UserRole.System.ToString();

        if (string.Equals(role, systemRole, StringComparison.InvariantCultureIgnoreCase) is false)
        {
            throw new InvalidRoleException(role);
        }

        return await next();
    }
}
