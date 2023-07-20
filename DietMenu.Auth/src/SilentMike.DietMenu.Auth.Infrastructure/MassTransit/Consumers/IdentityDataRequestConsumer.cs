// namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;
//
// using global::MassTransit;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
// using SilentMike.DietMenu.Auth.Infrastructure.Extensions;
// using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
// using SilentMike.DietMenu.Shared.Email.Models;
// using SilentMike.DietMenu.Shared.Identity.Interfaces;
// using SilentMike.DietMenu.Shared.Identity.Models;
// using IdentityOptions = SilentMike.DietMenu.Auth.Infrastructure.Identity.IdentityOptions;
//
// internal sealed class IdentityDataRequestConsumer : IConsumer<IIdentityDataRequest>
// {
//     private readonly IdentityOptions identityOptions;
//     private readonly ILogger<IdentityDataRequestConsumer> logger;
//     private readonly UserManager<DietMenuUser> userManager;
//
//     public IdentityDataRequestConsumer(IOptions<IdentityOptions> identityOptions, ILogger<IdentityDataRequestConsumer> logger, UserManager<DietMenuUser> userManager)
//     {
//         this.identityOptions = identityOptions.Value;
//         this.logger = logger;
//         this.userManager = userManager;
//     }
//
//     public async Task Consume(ConsumeContext<IIdentityDataRequest> context)
//     {
//         this.logger.LogInformation("Received identity data request");
//
//         if (context.ExpirationTime.HasValue && DateTime.UtcNow > context.ExpirationTime.Value.ToUniversalTime())
//         {
//             throw new TimeoutException();
//         }
//
//         if (context.Message.PayloadType == typeof(GetFamilyUserEmailPayload).FullName)
//         {
//             var payload = context.Message.Payload.To<GetFamilyUserEmailPayload>();
//
//             var userIdString = payload.FamilyId.ToString();
//
//             var user = await this.userManager.FindByIdAsync(userIdString);
//
//             if (user is null)
//             {
//                 throw new UserNotFoundException(payload.FamilyId);
//             }
//
//             var response = new GetFamilyUserEmailResponse
//             {
//                 Email = user.Email,
//             };
//
//             await context.RespondAsync(response);
//         }
//         else if (context.Message.PayloadType == typeof(VerifyUserEmailPayload).FullName)
//         {
//             var response = new GetSystemUserEmailResponse
//             {
//                 Email = this.identityOptions.SystemUserEmail,
//             };
//
//             await context.RespondAsync(response);
//         }
//         else
//         {
//             throw new FormatException("Unsupported email data payload type");
//         }
//     }
// }


