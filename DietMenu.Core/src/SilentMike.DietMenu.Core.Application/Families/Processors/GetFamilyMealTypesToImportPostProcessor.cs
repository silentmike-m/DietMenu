// namespace SilentMike.DietMenu.Core.Application.Families.Processors;
//
// using SilentMike.DietMenu.Core.Application.Exceptions;
// using SilentMike.DietMenu.Core.Application.Families.Interfaces;
// using SilentMike.DietMenu.Core.Domain.Common.Constants;
// using SilentMike.DietMenu.Core.Domain.Entities;
// using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;
//
// internal sealed class GetFamilyMealTypesToImportPostProcessor<TRequest, TResponse> : IGetFamilyDataToImportPostProcessor<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>, IGetFamilyDataToImport
//     where TResponse : IFamilyDataToImport
// {
//     private readonly string dataName = DataNames.MealTypes;
//
//     private readonly ILogger<GetFamilyMealTypesToImportPostProcessor<TRequest, TResponse>> logger;
//
//     public GetFamilyMealTypesToImportPostProcessor(ILogger<GetFamilyMealTypesToImportPostProcessor<TRequest, TResponse>> logger)
//         => this.logger = logger;
//
//     public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
//     {
//         this.logger.LogInformation("Try to import family {DataName", this.dataName);
//
//         try
//         {
//             if (string.Equals(request.Family[this.dataName], request.Core[this.dataName], StringComparison.InvariantCultureIgnoreCase))
//             {
//                 this.logger.LogInformation("Portfolio {DataName} is up to date", this.dataName);
//             }
//             else
//             {
//                 Import(request, response);
//
//                 request.Family[this.dataName] = request.Core[this.dataName];
//             }
//         }
//         catch (ApplicationException applicationException)
//         {
//             response.AddException(this.dataName, applicationException);
//         }
//         catch (Exception exception)
//         {
//             response.AddException(this.dataName, new UnknownErrorException(exception.Message));
//         }
//
//         await Task.CompletedTask;
//     }
//
//     private static void Import(TRequest request, TResponse response)
//     {
//         foreach (var coreMealType in request.CoreMealTypes)
//         {
//             var familyMealType = response.MealTypes
//                     .SingleOrDefault(type => type.InternalName == coreMealType.InternalName)
//                 ;
//
//             if (familyMealType is null)
//             {
//                 familyMealType = new MealTypeEntity(Guid.NewGuid())
//                 {
//                     FamilyId = request.Family.Id,
//                     InternalName = coreMealType.InternalName,
//                     IsActive = true,
//                     Name = coreMealType.Name,
//                     Order = coreMealType.Order,
//                 };
//
//                 response.MealTypes.Add(familyMealType);
//             }
//             else
//             {
//                 familyMealType.IsActive = true;
//                 familyMealType.Name = coreMealType.Name;
//                 familyMealType.Order = coreMealType.Order;
//             }
//         }
//     }
// }


