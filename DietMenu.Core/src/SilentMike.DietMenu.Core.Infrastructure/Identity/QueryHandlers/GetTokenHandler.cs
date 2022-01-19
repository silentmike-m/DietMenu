namespace SilentMike.DietMenu.Core.Infrastructure.Identity.QueryHandlers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SilentMike.DietMenu.Core.Application.Auth.Queries;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Exceptions.Auth;
using SilentMike.DietMenu.Core.Infrastructure.Identity.Models;

internal sealed class GetTokenHandler : IRequestHandler<GetToken, string>
{
    private readonly JwtOptions options;
    private readonly ILogger<GetTokenHandler> logger;
    private readonly UserManager<DietMenuUser> userManager;

    public GetTokenHandler(
        IOptions<JwtOptions> options,
        ILogger<GetTokenHandler> logger,
        UserManager<DietMenuUser> userManager)
    {
        this.options = options.Value;
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task<string> Handle(GetToken request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(("Email", request.Email));
        this.logger.LogInformation("Try to log user");

        var user = await this.userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new GetTokenException();
        }

        if (!user.EmailConfirmed)
        {
            throw new UnconfirmedEmailException(request.Email);
        }

        var result = await this.userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            throw new GetTokenException();
        }

        if (user.SecurityStamp == null)
        {
            await this.userManager.UpdateSecurityStampAsync(user);
        }

        var token = this.CreateToken(user);

        return token;
    }

    private string CreateToken(DietMenuUser user)
    {
        var familyId = user.FamilyId.ToString();
        var userId = user.Id.ToString();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(DietMenuClaimNames.FamilyId, familyId),
            new(DietMenuClaimNames.UserId, userId),
        };

        var dateTime = DateTime.Now;

        var token = new JwtSecurityToken
        (
            audience: this.options.Audience,
            claims: claims,
            issuer: this.options.Issuer,
            notBefore: dateTime,
            expires: dateTime.AddYears(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
