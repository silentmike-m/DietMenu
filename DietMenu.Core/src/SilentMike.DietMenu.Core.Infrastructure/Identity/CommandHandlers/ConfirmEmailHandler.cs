namespace SilentMike.DietMenu.Core.Infrastructure.Identity.CommandHandlers;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Events;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Exceptions.Auth;
using SilentMike.DietMenu.Core.Infrastructure.Identity.Models;

internal sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmail>
{
    private readonly ILogger<ConfirmEmailHandler> logger;
    private readonly IMediator mediator;
    private readonly UserManager<DietMenuUser> userManager;

    public ConfirmEmailHandler(ILogger<ConfirmEmailHandler> logger, IMediator mediator, UserManager<DietMenuUser> userManager)
        => (this.logger, this.mediator, this.userManager) = (logger, mediator, userManager);

    public async Task<Unit> Handle(ConfirmEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );

        this.logger.LogInformation("Try to confirm email");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is not null)
        {
            var result = await this.userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
            {
                throw new CreateUserException(result.Errors.First().Description);
            }

            var notification = new ConfirmedEmail
            {
                Email = request.Email,
            };

            await this.mediator.Publish(notification, cancellationToken);
        }

        return await Task.FromResult(Unit.Value);
    }
}
