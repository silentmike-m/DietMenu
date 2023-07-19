namespace SilentMike.DietMenu.Mailing.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Mailing.Application.Common;

[Serializable]
public sealed class ResourceNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.RESOURCE_NOT_FOUND;

    public ResourceNotFoundException(string resourceName)
        : base($"Resource with name '{resourceName}' has not been found")
    {
    }

    private ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
