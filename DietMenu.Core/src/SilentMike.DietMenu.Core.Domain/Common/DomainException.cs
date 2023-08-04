namespace SilentMike.DietMenu.Core.Domain.Common;

using System.Runtime.Serialization;

[Serializable]
public abstract class DomainException : Exception
{
    public abstract string Code { get; }
    public Guid Id { get; protected set; }

    protected DomainException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    protected DomainException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
