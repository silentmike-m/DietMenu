namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Services;
using global::AutoMapper;
using global::Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Models;

internal sealed class MealTypeReadService : IMealTypeReadService
{
    private const string GET_ALL_QUERY = "SELECT * FROM SilentMike.vMealTypes";
    private const string GET_ALL_COUNT_QUERY = "SELECT COUNT(1) FROM SilentMike.vMealTypes";

    private readonly IMapper mapper;
    private readonly DapperOptions options;

    public MealTypeReadService(IMapper mapper, IOptions<DapperOptions> options)
        => (this.mapper, this.options) = (mapper, options.Value);

    public async Task<MealTypesGrid> GetMealTypesGridAsync(Guid familyId, GridRequest gridRequest, CancellationToken cancellationToken = default)
    {
        var filters = GetFilters(familyId, gridRequest.Filter);
        var orderBy = GetOrderBy(gridRequest.OrderBy);

        var selectQuery = new Query(GET_ALL_QUERY)
            .GetFiltered(filters)
            .GetOrdered(orderBy, gridRequest.IsDescending)
            .GetPaged(gridRequest.PageNumber, gridRequest.IsPaged, gridRequest.PageSize);

        var countQuery = new Query(GET_ALL_COUNT_QUERY)
                .GetFiltered(filters);

        var query = $"{selectQuery.QueryBuilder} {countQuery.QueryBuilder}";

        await using var connection = new SqlConnection(this.options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        using var multi = await connection.QueryMultipleAsync(query, cancellationToken);

        var types = multi.Read<MealTypeEntity>().ToList();
        var count = multi.Read<int>().First();

        var mealTypes = this.mapper.Map<IReadOnlyList<MealType>>(types);

        var result = new MealTypesGrid
        {
            Count = count,
            Elements = mealTypes,
        };

        return await Task.FromResult(result);
    }

    private static List<Filter> GetFilters(Guid familyId, string filter)
    {
        var familyFilter = new Filter
        {
            FilterJoinType = FilterJoinType.And,
            FilterType = FilterType.Equals,
            IsString = true,
            PropertyName = nameof(MealTypeEntity.FamilyId),
            Value = familyId.ToString(),
        };

        var nameFilter = new Filter
        {
            FilterJoinType = FilterJoinType.And,
            FilterType = FilterType.Contains,
            IsString = true,
            PropertyName = nameof(MealTypeEntity.Name),
            Value = filter,
        };

        return new List<Filter>
        {
            familyFilter,
            nameFilter,
        };
    }

    private static string GetOrderBy(string orderBy)
    {
        return orderBy.ToLower() switch
        {
            "name" => nameof(MealTypeEntity.Name),
            _ => nameof(MealTypeEntity.Order),
        };
    }
}











