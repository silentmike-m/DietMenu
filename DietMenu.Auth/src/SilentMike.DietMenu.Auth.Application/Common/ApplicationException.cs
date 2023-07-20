namespace SilentMike.DietMenu.Auth.Application.Common;

using System.Runtime.Serialization;

[Serializable]
public abstract class ApplicationException : Exception
{
    public abstract string Code { get; }
    public Guid Id { get; protected set; }

    protected ApplicationException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }

    protected ApplicationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
