namespace DietMenu.Core.Infrastructure.Identity.CommandHandlers;

using DietMenu.Core.Application.Common;
using DietMenu.Core.Application.Common.Constants;
using DietMenu.Core.Application.Exceptions;
using DietMenu.Core.Application.Exceptions.Families;
using DietMenu.Core.Application.Exceptions.Users;
using DietMenu.Core.Application.Users.Commands;
using DietMenu.Core.Application.Users.Events;
using DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using DietMenu.Core.Infrastructure.Identity.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IdentityOptions = DietMenu.Core.Infrastructure.Identity.IdentityOptions;

internal sealed class CreateUserHandler : IRequestHandler<CreateUser>
{
    private readonly IdentityOptions options;

    private readonly IApplicationDbContext context;
    private readonly ILogger<CreateUserHandler> logger;
    private readonly IMediator mediator;
    private readonly UserManager<DietMenuUser> userManager;

    public CreateUserHandler(
        IConfiguration configuration,
        IApplicationDbContext context,
        ILogger<CreateUserHandler> logger,
        IMediator mediator,
        UserManager<DietMenuUser> userManager)
    {
        this.options = configuration.GetSection(IdentityOptions.SECTION_NAME).Get<IdentityOptions>();

        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyName", request.User.FamilyName),
            ("UserId", request.User.Id)
        );

        this.logger.LogInformation("Try to create user");

        if (request.CreateCode != this.options.CreateUserCode)
        {
            var validationFailure = new ValidationFailure(nameof(request.CreateCode), ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE,
            };

            throw new ValidationException(new List<ValidationFailure>
            {
                validationFailure,
            });
        }

        var user = await this.userManager.FindByIdAsync(request.User.Id.ToString())
                   ?? await this.userManager.FindByEmailAsync(request.User.Email);

        if (user is not null)
        {
            throw new CreateUserException();
        }

        var family = await this.context.Families
            .SingleOrDefaultAsync(i => i.Name == request.User.FamilyName, cancellationToken);

        if (family is not null)
        {
            throw new FamilyAlreadyExistsException(request.User.FamilyName);
        }

        user = new DietMenuUser
        {
            Email = request.User.Email,
            FamilyId = Guid.NewGuid(),
            FirstName = request.User.FirstName,
            Id = request.User.Id,
            LastName = request.User.LastName,
            IsSystem = false,
            NormalizedUserName = request.User.UserName.ToUpper(),
            UserName = request.User.UserName,
        };

        var result = await this.userManager.CreateAsync(user, request.User.Password);

        if (!result.Succeeded)
        {
            throw new CreateUserException(result.Errors.First().Description);
        }

        await this.mediator.Publish(new CreatedUser
        {
            FamilyId = user.FamilyId,
            FamilyName = request.User.FamilyName,
            UserId = user.Id,
        }, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
