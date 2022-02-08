namespace SilentMike.DietMenu.Core.Application.IngredientTypes.CommandHandlers;

using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Events;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class UpsertIngredientTypeHandler : IRequestHandler<UpsertIngredientType>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<UpsertIngredientTypeHandler> logger;
    private readonly IMediator mediator;
    private readonly IIngredientTypeRepository typeRepository;

    public UpsertIngredientTypeHandler(
        IFamilyRepository familyRepository,
        ILogger<UpsertIngredientTypeHandler> logger,
        IMediator mediator,
        IIngredientTypeRepository typeRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.typeRepository = typeRepository;
    }

    public async Task<Unit> Handle(UpsertIngredientType request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to upsert ingredient types");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var ingredientType = await this.typeRepository.Get(request.IngredientType.Id, cancellationToken);

        if (ingredientType is null)
        {
            ingredientType = this.Create(request.FamilyId, request.IngredientType);
        }
        else
        {
            this.Update(ingredientType, request.IngredientType);
        }

        await this.typeRepository.Save(ingredientType, cancellationToken);

        var notification = new UpsertedIngredientType
        {
            FamilyId = request.FamilyId,
            Id = ingredientType.Id,
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private IngredientTypeEntity Create(Guid requestFamilyId, IngredientTypeToUpsert ingredientTypeToUpsert)
    {
        this.logger.LogInformation("Try to create ingredient type with id {IngredientTypeId}", ingredientTypeToUpsert.Id);

        ValidateNewType(ingredientTypeToUpsert);

        var ingredientType = new IngredientTypeEntity(ingredientTypeToUpsert.Id)
        {
            FamilyId = requestFamilyId,
            InternalName = ingredientTypeToUpsert.Id.ToString(),
            IsSystem = false,
            Name = ingredientTypeToUpsert.Name!,
        };

        return ingredientType;
    }

    private void Update(IngredientTypeEntity ingredientType, IngredientTypeToUpsert ingredientTypeToUpsert)
    {
        this.logger.LogInformation("Try to update ingredient type with id {IngredientTypeId}", ingredientTypeToUpsert.Id);

        ingredientType.Name = ingredientTypeToUpsert.Name ?? ingredientType.Name;
    }

    private static void ValidateNewType(IngredientTypeToUpsert ingredientTypeToUpsert)
    {
        var errors = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(ingredientTypeToUpsert.Name))
        {
            errors.Add(new ValidationFailure(nameof(ingredientTypeToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENT_TYPE_EMPTY_NAME,
            });
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
