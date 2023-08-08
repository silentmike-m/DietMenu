namespace SilentMike.DietMenu.Core.Application.Common.Behaviors;

using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Domain.Services;

internal sealed class FamilyRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, IFamilyRequest
{
    private readonly IFamilyRepository familyRepository;

    public FamilyRequestBehavior(IFamilyRepository familyRepository)
        => this.familyRepository = familyRepository;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var exists = await this.familyRepository.ExistsAsync(request.FamilyId, cancellationToken);

        if (exists is false)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        return await next();
    }
}
