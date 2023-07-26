namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;

public interface IIdentityPageUrlService
{
    string GetConfirmUserEmailPageUrl(Uri host, string token, Guid userId);
}
