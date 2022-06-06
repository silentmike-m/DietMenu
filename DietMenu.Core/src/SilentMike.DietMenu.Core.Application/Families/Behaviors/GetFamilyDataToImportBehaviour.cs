namespace SilentMike.DietMenu.Core.Application.Families.Behaviors;

using System.Diagnostics.CodeAnalysis;
using SilentMike.DietMenu.Core.Application.Families.Interfaces;

[ExcludeFromCodeCoverage]
internal sealed class GetFamilyDataToImportBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IGetFamilyDataToImport
    where TResponse : IFamilyDataToImport
{
    private readonly IEnumerable<IGetFamilyDataToImportPostProcessor<TRequest, TResponse>> postProcessors;

    public GetFamilyDataToImportBehaviour(IEnumerable<IGetFamilyDataToImportPostProcessor<TRequest, TResponse>> postProcessors)
        => this.postProcessors = postProcessors;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var response = await next();

        foreach (var processor in this.postProcessors)
        {
            await processor.Process(request, response, cancellationToken);
        }

        return response;
    }
}
