namespace SilentMike.DietMenu.Core.Application.Common.Behaviors;

using SilentMike.DietMenu.Core.Application.Exceptions;

internal sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAuthRequest
{
    private readonly IAuthService currentUserService;

    public AuthorizationBehavior(IAuthService currentUserService)
        => this.currentUserService = currentUserService;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
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
