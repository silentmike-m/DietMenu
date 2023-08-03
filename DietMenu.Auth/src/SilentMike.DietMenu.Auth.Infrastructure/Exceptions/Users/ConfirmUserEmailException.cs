namespace SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Infrastructure.Common;

[Serializable]
public sealed class ConfirmUserEmailException : InfrastructureException
{
    public override string Code => ErrorCodes.CONFIRM_USER_EMAIL_ERROR;

    public ConfirmUserEmailException(Guid id, string message)
        : base($"Unable to confirm email user with id '{id}': {message}")
        => this.Id = id;

    private ConfirmUserEmailException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
