namespace SilentMike.DietMenu.Auth.UnitTests.Families.QueryHandlers;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.Families.QueryHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class GetFamilyEmailHandlerTests : FakeDietMenuDbContext
{
    private static readonly Family EXISTING_FAMILY = new()
    {
        Email = "family@domain.com",
        Id = Guid.NewGuid(),
        Key = 1,
        Name = "family name",
    };

    private readonly NullLogger<GetFamilyEmailHandler> logger = new();

    public GetFamilyEmailHandlerTests()
        : base(EXISTING_FAMILY)
    {
    }

    [TestMethod]
    public async Task Should_Return_Family_Email()
    {
        //GIVEN
        var request = new GetFamilyEmail
        {
            FamilyId = EXISTING_FAMILY.Id,
        };

        var handler = new GetFamilyEmailHandler(this.Context!, this.logger);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        result.Should()
            .NotBeNull()
            .And
            .Be(EXISTING_FAMILY.Email)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Family_Not_Found_Exception_When_Missing_Family()
    {
        //GIVEN
        var request = new GetFamilyEmail
        {
            FamilyId = Guid.NewGuid(),
        };

        var handler = new GetFamilyEmailHandler(this.Context!, this.logger);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_NOT_FOUND)
                .Where(exception => exception.Id == request.FamilyId)
            ;
    }
}
