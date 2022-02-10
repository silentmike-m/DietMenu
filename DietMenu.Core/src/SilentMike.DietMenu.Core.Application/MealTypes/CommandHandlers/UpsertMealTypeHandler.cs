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

internal sealed class UpsertMealTypeHandler : IRequestHandler<UpsertMealType>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<UpsertMealTypeHandler> logger;
    private readonly IMediator mediator;
    private readonly IMealTypeRepository mealTypeRepository;

    public UpsertMealTypeHandler(
        IFamilyRepository familyRepository,
        ILogger<UpsertMealTypeHandler> logger,
        IMediator mediator,
        IMealTypeRepository mealTypeRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.mealTypeRepository = mealTypeRepository;
    }

    public async Task<Unit> Handle(UpsertMealType request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("MealTypeId", request.MealType.Id)
        );

        this.logger.LogInformation("Try to upsert meal type");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var mealType = await this.mealTypeRepository.Get(request.MealType.Id, cancellationToken);

        if (mealType is null)
        {
            mealType = this.CreateMealType(request.FamilyId, request.MealType);
        }
        else
        {
            this.UpdateMealType(mealType, request.MealType);
        }

        await this.mealTypeRepository.Save(mealType, cancellationToken);

        var notification = new UpsertedMealType
        {
            FamilyId = request.FamilyId,
            Id = mealType.Id,
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private MealTypeEntity CreateMealType(Guid familyId, MealTypeToUpsert mealTypeToUpsert)
    {
        this.logger.LogInformation("Try to create meal type");

        ValidateNewMealType(mealTypeToUpsert);

        var mealType = new MealTypeEntity(mealTypeToUpsert.Id)
        {
            FamilyId = familyId,
            InternalName = mealTypeToUpsert.Id.ToString(),
            Name = mealTypeToUpsert.Name!,
            Order = mealTypeToUpsert.Order!.Value,
        };

        return mealType;
    }

    private void UpdateMealType(MealTypeEntity mealType, MealTypeToUpsert mealTypeToUpsert)
    {
        this.logger.LogInformation("Try to update meal type");

        mealType.Name = mealTypeToUpsert.Name ?? mealType.Name;
        mealType.Order = mealTypeToUpsert.Order ?? mealType.Order;
    }

    private static void ValidateNewMealType(MealTypeToUpsert mealTypeToUpsert)
    {
        var errors = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(mealTypeToUpsert.Name))
        {
            errors.Add(new ValidationFailure(nameof(mealTypeToUpsert.Name), ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_MEAL_TYPE_EMPTY_NAME,
            });
        }

        if (mealTypeToUpsert.Order is null or < 1)
        {
            errors.Add(new ValidationFailure(nameof(mealTypeToUpsert.Order), ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_MEAL_TYPE_INVALID_ORDER,
            });
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
