namespace SilentMike.DietMenu.Core.Application.MealTypes.CommandHandlers;

using FluentValidation.Results;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.Application.MealTypes.Events;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class UpsertMealTypesHandler : IRequestHandler<UpsertMealTypes>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<UpsertMealTypesHandler> logger;
    private readonly IMediator mediator;
    private readonly IMealTypeRepository mealTypeRepository;

    public UpsertMealTypesHandler(
        IFamilyRepository familyRepository,
        ILogger<UpsertMealTypesHandler> logger,
        IMediator mediator,
        IMealTypeRepository mealTypeRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.mealTypeRepository = mealTypeRepository;
    }

    public async Task<Unit> Handle(UpsertMealTypes request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to upsert meal types");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        foreach (var mealTypeToUpsert in request.MealTypes)
        {
            var mealType = await this.mealTypeRepository.Get(request.FamilyId, mealTypeToUpsert.Id, cancellationToken);

            if (mealType is null)
            {
                await this.CreateMealType(request.FamilyId, mealTypeToUpsert, cancellationToken);
            }
            else
            {
                await this.UpdateMealType(mealType, mealTypeToUpsert, cancellationToken);
            }
        }

        var ids = request.MealTypes
            .Select(i => i.Id)
            .ToList();

        var notification = new UpsertedMealTypes
        {
            FamilyId = request.FamilyId,
            Ids = ids.AsReadOnly(),
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private async Task CreateMealType(Guid familyId, MealTypeToUpsert mealTypeToUpsert, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to create meal type with id {MealTypeId}", mealTypeToUpsert.Id);

        ValidateNewMealType(mealTypeToUpsert);

        var mealType = new MealTypeEntity(mealTypeToUpsert.Id)
        {
            FamilyId = familyId,
            InternalName = mealTypeToUpsert.Id.ToString(),
            Name = mealTypeToUpsert.Name ?? string.Empty,
            Order = mealTypeToUpsert.Order ?? 1,
        };

        await this.mealTypeRepository.Save(mealType, cancellationToken);
    }

    private async Task UpdateMealType(MealTypeEntity mealType, MealTypeToUpsert mealTypeToUpsert, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to update meal type with id {MealTypeId}", mealTypeToUpsert.Id);

        mealType.Name = mealTypeToUpsert.Name ?? mealType.Name;
        mealType.Order = mealTypeToUpsert.Order ?? mealType.Order;

        await this.mealTypeRepository.Save(mealType, cancellationToken);
    }

    private static void ValidateNewMealType(MealTypeToUpsert mealTypeToUpsert)
    {
        var errors = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(mealTypeToUpsert.Name))
        {
            errors.Add(new ValidationFailure(nameof(mealTypeToUpsert.Name), ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_MEAL_TYPES_EMPTY_NAME,
            });
        }

        if (mealTypeToUpsert.Order is null or < 1)
        {
            errors.Add(new ValidationFailure(nameof(mealTypeToUpsert.Order), ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_MEAL_TYPES_INVALID_ORDER,
            });
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
