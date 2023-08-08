namespace SilentMike.DietMenu.Core.Application.Common.Behaviors;

using SilentMike.DietMenu.Core.Application.Exceptions;

internal sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, IAuthRequest
{
    private readonly IAuthService currentUserService;

    public AuthorizationBehavior(IAuthService currentUserService)
        => this.currentUserService = currentUserService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.AuthData.FamilyId == Guid.Empty || request.AuthData.UserId == Guid.Empty)
        {
            var (familyId, userId) = this.currentUserService.CurrentUser;

            if (familyId == Guid.Empty || userId == Guid.Empty)
            {
                throw new DietMenuUnauthorizedException(familyId, userId);
            }

            if (request.AuthData.FamilyId == Guid.Empty)
            {
                request.AuthData.FamilyId = familyId;
            }

            if (request.AuthData.UserId == Guid.Empty)
            {
                request.AuthData.UserId = userId;
            }
        }

        return await next();
    }
}
