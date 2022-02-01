namespace SilentMike.DietMenu.Core.Application.Common.Behaviours;

using SilentMike.DietMenu.Core.Application.Exceptions;

internal sealed class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentRequestService currentUserService;

    public AuthorizationBehaviour(ICurrentRequestService currentUserService)
    {
        this.currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request is IAuthRequest authId)
        {
            var (familyId, userId) = this.currentUserService.CurrentUser;

            if (familyId == Guid.Empty || userId == Guid.Empty)
            {
                throw new DietMenuUnauthorizedException(familyId, userId);
            }

            authId.FamilyId = familyId;
            authId.UserId = userId;
        }

        return await next();
    }
}
