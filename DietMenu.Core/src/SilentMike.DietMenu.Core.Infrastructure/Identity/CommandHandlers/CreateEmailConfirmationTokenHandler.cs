namespace SilentMike.DietMenu.Core.Infrastructure.Identity.CommandHandlers;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Events;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Infrastructure.Identity.Models;

internal sealed class CreateEmailConfirmationTokenHandler : IRequestHandler<CreateEmailConfirmationToken>
{
    private readonly ILogger<CreateEmailConfirmationTokenHandler> logger;
    private readonly IMediator mediator;
    private readonly UserManager<DietMenuUser> userManager;

    public CreateEmailConfirmationTokenHandler(
        ILogger<CreateEmailConfirmationTokenHandler> logger,
        IMediator mediator,
        UserManager<DietMenuUser> userManager)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(CreateEmailConfirmationToken request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );

        this.logger.LogInformation("Try to create email confirmation token");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is not null)
        {
            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

            var notification = new CreatedEmailConfirmationToken
            {
                Email = request.Email,
                Token = token,
                UserName = user.UserName,
            };

            await this.mediator.Publish(notification, cancellationToken);
        }

        return await Task.FromResult(Unit.Value);
    }
}
