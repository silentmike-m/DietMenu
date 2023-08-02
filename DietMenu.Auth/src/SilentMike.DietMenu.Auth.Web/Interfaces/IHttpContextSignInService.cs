namespace SilentMike.DietMenu.Auth.Web.Interfaces;

public interface IHttpContextSignInService
{
    Task SignInAsync(string email, CancellationToken cancellationToken = default);
}
