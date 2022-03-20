namespace SilentMike.DietMenu.Core.Application.Families.Interfaces;

internal interface IGetFamilyDataToImportPostProcessor<in TRequest, in TResponse>
    where TRequest : IRequest<TResponse>, IGetFamilyDataToImport
    where TResponse : IFamilyDataToImport
{
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}
