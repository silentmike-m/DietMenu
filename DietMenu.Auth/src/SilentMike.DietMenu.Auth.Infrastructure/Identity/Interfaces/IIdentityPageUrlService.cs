namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;

public interface IIdentityPageUrlService
{
    string GetConfirmUserEmailPageUrl(Uri hostUri, Uri returnHostUri, string token, Guid userId);
}
