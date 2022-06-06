namespace SilentMike.DietMenu.Core.Infrastructure.Ingredients.QueryHandlers;

using global::AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

internal sealed record GetIngredientHandler : IRequestHandler<GetIngredient, Ingredient>
{
    private readonly DietMenuDbContext context;
    private readonly ILogger<GetIngredientHandler> logger;
    private readonly IMapper mapper;

    public GetIngredientHandler(DietMenuDbContext context, ILogger<GetIngredientHandler> logger, IMapper mapper)
        => (this.context, this.logger, this.mapper) = (context, logger, mapper);

    public async Task<Ingredient> Handle(GetIngredient request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("IngredientId", request.Id)
        );

        this.logger.LogInformation("Try to get ingredient");

        var ingredient = await this.context.IngredientRows
            .Where(ingredient => ingredient.FamilyId == request.FamilyId)
            .SingleOrDefaultAsync(ingredient => ingredient.Id == request.Id, cancellationToken);

        if (ingredient is null)
        {
            throw new IngredientNotFoundException(request.Id);
        }

        var result = this.mapper.Map<Ingredient>(ingredient);

        return result;
    }
}
