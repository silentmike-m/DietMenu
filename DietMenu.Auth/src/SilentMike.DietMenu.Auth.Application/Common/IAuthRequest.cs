namespace SilentMike.DietMenu.Auth.Application.Common;

public interface IAuthRequest
{
    IAuthData AuthData { get; set; }
}
