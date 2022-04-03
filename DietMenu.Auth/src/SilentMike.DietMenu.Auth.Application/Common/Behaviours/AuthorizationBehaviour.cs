namespace SilentMike.DietMenu.Auth.Application.Common.Behaviours;

using System.Diagnostics.CodeAnalysis;
using SilentMike.DietMenu.Auth.Application.Exceptions;

[ExcludeFromCodeCoverage]
internal sealed class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAuthRequest
{
    private readonly ICurrentRequestService currentUserService;

    public AuthorizationBehaviour(ICurrentRequestService currentUserService)
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
