namespace SilentMike.DietMenu.Core.Application.Core.Interfaces;

internal interface IGetCoreDataToImportPostProcessor<in TRequest, in TResponse>
    where TRequest : IRequest<TResponse>, IGetCoreDataToImport
    where TResponse : ICoreDataToImport
{
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}
