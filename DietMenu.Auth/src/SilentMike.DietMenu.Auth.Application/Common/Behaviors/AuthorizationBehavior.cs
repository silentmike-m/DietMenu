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
        if (request.FamilyId == Guid.Empty || request.UserId == Guid.Empty)
        {
            var (portfolioId, userId) = this.currentUserService.CurrentUser;

            if (request.FamilyId == Guid.Empty)
            {
                request.FamilyId = portfolioId;
            }

            if (request.UserId == Guid.Empty)
            {
                request.UserId = userId;
            }

            if (request.FamilyId == Guid.Empty && request.UserId == Guid.Empty)
            {
                throw new DietMenuUnauthorizedException(portfolioId, userId);
            }
        }

        return await next();
    }
}
