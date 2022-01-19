namespace SilentMike.DietMenu.Core.Infrastructure.Identity.CommandHandlers;

using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.Users;
using SilentMike.DietMenu.Core.Application.Users.Commands;
using SilentMike.DietMenu.Core.Application.Users.Events;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.Identity.Models;
using IdentityOptions = global::SilentMike.DietMenu.Core.Infrastructure.Identity.IdentityOptions;

internal sealed class CreateUserHandler : IRequestHandler<CreateUser>
{
    private readonly IApplicationDbContext context;
    private readonly ILogger<CreateUserHandler> logger;
    private readonly IMediator mediator;
    private readonly IdentityOptions options;
    private readonly UserManager<DietMenuUser> userManager;

    public CreateUserHandler(
        IApplicationDbContext context,
        ILogger<CreateUserHandler> logger,
        IMediator mediator,
        IOptions<IdentityOptions> options,
        UserManager<DietMenuUser> userManager)
    {
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
        this.options = options.Value;
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

        var notification = new CreatedUser
        {
            Email = user.Email,
            FamilyId = user.FamilyId,
            FamilyName = request.User.FamilyName,
            Id = user.Id,
            LoginUrl = this.options.LoginUrl,
            UserName = user.UserName,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
