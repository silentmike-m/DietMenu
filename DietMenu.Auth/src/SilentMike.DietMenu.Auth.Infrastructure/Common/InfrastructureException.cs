namespace SilentMike.DietMenu.Auth.Infrastructure.Common;

using System.Runtime.Serialization;

[Serializable]
public abstract class InfrastructureException : Exception
{
    public abstract string Code { get; }
    public Guid Id { get; protected set; }

    protected InfrastructureException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }

    protected InfrastructureException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
