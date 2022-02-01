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

internal sealed class UpsertIngredientTypesHandler : IRequestHandler<UpsertIngredientTypes>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<UpsertIngredientTypesHandler> logger;
    private readonly IMediator mediator;
    private readonly IIngredientTypeRepository typeRepository;

    public UpsertIngredientTypesHandler(
        IFamilyRepository familyRepository,
        ILogger<UpsertIngredientTypesHandler> logger,
        IMediator mediator,
        IIngredientTypeRepository typeRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.typeRepository = typeRepository;
    }

    public async Task<Unit> Handle(UpsertIngredientTypes request, CancellationToken cancellationToken)
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

        foreach (var ingredientTypeToUpsert in request.IngredientTypes)
        {
            var ingredientType = await this.typeRepository.Get(request.FamilyId, ingredientTypeToUpsert.Id, cancellationToken);

            if (ingredientType is null)
            {
                await this.Create(request.FamilyId, ingredientTypeToUpsert, cancellationToken);
            }
            else
            {
                await this.Update(ingredientType, ingredientTypeToUpsert, cancellationToken);
            }
        }

        var ids = request.IngredientTypes
            .Select(i => i.Id)
            .ToList();

        var notification = new UpsertedIngredientTypes
        {
            FamilyId = request.FamilyId,
            Ids = ids.AsReadOnly(),
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private async Task Create(Guid requestFamilyId, IngredientTypeToUpsert ingredientTypeToUpsert, CancellationToken cancellationToken)
    {
        ValidateNewType(ingredientTypeToUpsert);

        var ingredientType = new IngredientTypeEntity(ingredientTypeToUpsert.Id)
        {
            FamilyId = requestFamilyId,
            InternalName = ingredientTypeToUpsert.Id.ToString(),
            IsSystem = false,
            Name = ingredientTypeToUpsert.Name ?? string.Empty,
        };

        await this.typeRepository.Save(ingredientType, cancellationToken);
    }

    private async Task Update(
        IngredientTypeEntity ingredientType,
        IngredientTypeToUpsert ingredientTypeToUpsert,
        CancellationToken cancellationToken)
    {
        ingredientType.Name = ingredientTypeToUpsert.Name ?? ingredientType.Name;

        await this.typeRepository.Save(ingredientType, cancellationToken);
    }

    private static void ValidateNewType(IngredientTypeToUpsert ingredientTypeToUpsert)
    {
        var errors = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(ingredientTypeToUpsert.Name))
        {
            errors.Add(new ValidationFailure(nameof(ingredientTypeToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENT_TYPES_EMPTY_NAME,
            });
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}