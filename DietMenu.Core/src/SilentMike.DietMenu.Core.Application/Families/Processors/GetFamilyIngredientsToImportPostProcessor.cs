// namespace SilentMike.DietMenu.Core.Application.Families.Processors;
//
// using SilentMike.DietMenu.Core.Application.Common;
// using SilentMike.DietMenu.Core.Application.Exceptions;
// using SilentMike.DietMenu.Core.Application.Families.Interfaces;
//
// internal sealed class GetFamilyIngredientsToImportPostProcessor<TRequest, TResponse> : IGetFamilyDataToImportPostProcessor<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>, IGetFamilyDataToImport
//     where TResponse : IFamilyDataToImport
// {
//     private readonly string defaultDataName = DataNames.Ingredients;
//
//     private readonly ILogger<GetFamilyIngredientsToImportPostProcessor<TRequest, TResponse>> logger;
//
//     public GetFamilyIngredientsToImportPostProcessor(ILogger<GetFamilyIngredientsToImportPostProcessor<TRequest, TResponse>> logger)
//         => this.logger = logger;
//
//     public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
//     {
//         this.logger.LogInformation("Try to import family {DataName", this.defaultDataName);
//
//         try
//         {
//             foreach (var ingredientType in request.CoreIngredientTypes)
//             {
//                 Import(ingredientType, request, response);
//             }
//         }
//         catch (ApplicationException applicationException)
//         {
//             response.AddException(this.defaultDataName, applicationException);
//         }
//         catch (Exception exception)
//         {
//             response.AddException(this.defaultDataName, new UnknownErrorException(exception.Message));
//         }
//
//         await Task.CompletedTask;
//     }
//
//     private static void DeactivateIngredients(IReadOnlyCollection<CoreIngredientEntity> coreIngredients, List<IngredientEntity> familyIngredients)
//     {
//         foreach (var familyIngredient in familyIngredients)
//         {
//             var coreIngredient = coreIngredients
//                 .SingleOrDefault(ingredient => ingredient.InternalName == familyIngredient.InternalName);
//
//             if (coreIngredient is null)
//             {
//                 familyIngredient.IsActive = false;
//             }
//         }
//     }
//
//     private static void Import(CoreIngredientTypeEntity ingredientType, TRequest request, TResponse response)
//     {
//         try
//         {
//             var familyIngredientType = response.IngredientTypes
//                 .SingleOrDefault(type => type.InternalName == ingredientType.InternalName);
//
//             if (familyIngredientType is null)
//             {
//                 throw new IngredientTypeNotFoundException(ingredientType.InternalName);
//             }
//
//             var coreIngredients = request.CoreIngredients
//                 .Where(ingredient => ingredient.TypeId == ingredientType.Id)
//                 .ToList();
//
//             var familyIngredients = response.Ingredients
//                 .Where(ingredient => ingredient.TypeId == familyIngredientType.Id)
//                 .ToList();
//
//             DeactivateIngredients(coreIngredients, familyIngredients);
//
//             foreach (var coreIngredient in coreIngredients)
//             {
//                 var familyIngredient = familyIngredients
//                     .SingleOrDefault(ingredient => ingredient.InternalName == coreIngredient.InternalName);
//
//                 if (familyIngredient is null)
//                 {
//                     familyIngredient = new IngredientEntity(Guid.NewGuid())
//                     {
//                         Exchanger = coreIngredient.Exchanger,
//                         FamilyId = request.Family.Id,
//                         InternalName = coreIngredient.InternalName,
//                         IsActive = true,
//                         IsSystem = true,
//                         Name = coreIngredient.Name,
//                         TypeId = familyIngredientType.Id,
//                         UnitSymbol = coreIngredient.UnitSymbol,
//                     };
//
//                     response.Ingredients.Add(familyIngredient);
//                 }
//                 else
//                 {
//                     familyIngredient.Exchanger = coreIngredient.Exchanger;
//                     familyIngredient.IsActive = true;
//                     familyIngredient.Name = coreIngredient.Name;
//                     familyIngredient.UnitSymbol = coreIngredient.UnitSymbol;
//                 }
//             }
//
//             request.Family[ingredientType.InternalName] = request.Core[ingredientType.InternalName];
//         }
//         catch (ApplicationException applicationException)
//         {
//             response.AddException(ingredientType.InternalName, applicationException);
//         }
//         catch (Exception exception)
//         {
//             response.AddException(ingredientType.InternalName, new UnknownErrorException(exception.Message));
//         }
//     }
// }
