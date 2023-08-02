namespace SilentMike.DietMenu.Auth.Application.Common.Behaviors;

using SilentMike.DietMenu.Auth.Application.Exceptions;

internal sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, IAuthRequest
{
    private readonly ICurrentRequestService currentUserService;

    public AuthorizationBehavior(ICurrentRequestService currentUserService)
        => this.currentUserService = currentUserService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.AuthData.FamilyId == Guid.Empty || request.AuthData.UserId == Guid.Empty)
        {
            var (familyId, userId) = this.currentUserService.CurrentUser;

            if (request.AuthData.FamilyId == Guid.Empty)
            {
                request.AuthData.FamilyId = familyId;
            }

            if (request.AuthData.UserId == Guid.Empty)
            {
                request.AuthData.UserId = userId;
            }

            if (request.AuthData.FamilyId == Guid.Empty || request.AuthData.UserId == Guid.Empty)
            {
                throw new DietMenuUnauthorizedException(familyId, userId);
            }
        }

        return await next();
    }
}
