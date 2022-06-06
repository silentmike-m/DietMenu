namespace SilentMike.DietMenu.Core.Application.Core.Behaviors;

using System.Diagnostics.CodeAnalysis;
using SilentMike.DietMenu.Core.Application.Core.Interfaces;

[ExcludeFromCodeCoverage]
internal sealed class GetCoreDataToImportBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IGetCoreDataToImport
    where TResponse : ICoreDataToImport
{
    private readonly IEnumerable<IGetCoreDataToImportPostProcessor<TRequest, TResponse>> postProcessors;

    public GetCoreDataToImportBehaviour(IEnumerable<IGetCoreDataToImportPostProcessor<TRequest, TResponse>> postProcessors)
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
