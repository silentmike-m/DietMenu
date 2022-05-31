namespace SilentMike.DietMenu.Core.Application.Exceptions;

using System.Runtime.Serialization;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class ResourceNotFoundException : ApplicationException
{
    public override string Code => ErrorCodes.RESOURCE_NOT_FOUND;

    public ResourceNotFoundException(string resourceName)
        : base($"Resource with name '{resourceName} has not been found'")
    {
    }

    private ResourceNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
