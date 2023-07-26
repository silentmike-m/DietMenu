namespace SilentMike.DietMenu.Auth.Domain.Services;

using SilentMike.DietMenu.Auth.Domain.Entities;

public interface IUserService
{
    Task CreateUserAsync(string password, UserEntity user, CancellationToken cancellationToken = default);
    Task<string?> GenerateEmailConfirmationTokenAsync(UserEntity user, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
