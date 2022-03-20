namespace SilentMike.DietMenu.Core.Application.Common.Behaviours;

using SilentMike.DietMenu.Core.Application.Exceptions;

internal sealed class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAuthRequest
{
    private readonly ICurrentRequestService currentUserService;

    public AuthorizationBehaviour(ICurrentRequestService currentUserService)
    {
        this.currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var (familyId, userId) = this.currentUserService.CurrentUser;

        if (familyId == Guid.Empty || userId == Guid.Empty)
        {
            throw new DietMenuUnauthorizedException(familyId, userId);
        }

        request.FamilyId = familyId;
        request.UserId = userId;

        return await next();
    }
}
