namespace SilentMike.DietMenu.Core.Application.Common.Behaviors;

internal sealed class FamilyRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, IFamilyRequest
{
    // private readonly IFamilyRepository familyRepository;
    //
    // public FamilyRequestBehavior(IFamilyRepository familyRepository)
    //     => this.familyRepository = familyRepository;
    //
    // public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    // {
    //     var exists = this.familyRepository.Exists(request.FamilyId);
    //
    //     if (exists is false)
    //     {
    //         throw new FamilyNotFoundException(request.FamilyId);
    //     }
    //
    //     return await next();
    // }
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
