namespace DietMenu.Core.Application.Exceptions.Users;

using System;
using System.Runtime.Serialization;
using DietMenu.Core.Application.Common.Constants;

[Serializable]
public sealed class CreateUserException : Core.Application.Common.ApplicationException
{
    public override string Code => ErrorCodes.CREATE_USER;

    public CreateUserException()
        : base("A link to activate your account has been emailed to the address provided.")
    {
        this.Id = Guid.Empty;
    }

    public CreateUserException(string message)
        : base($"Unable to create user: {message}")
    {
        this.Id = Guid.Empty;
    }

    private CreateUserException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
