namespace SilentMike.DietMenu.Core.Application.Common.Behaviors;

using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class FamilyRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IFamilyRequest
{
    private readonly IFamilyRepository familyRepository;

    public FamilyRequestBehavior(IFamilyRepository familyRepository)
        => this.familyRepository = familyRepository;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var exists = this.familyRepository.Exists(request.FamilyId);

        if (!exists)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        return await next();
    }
}
