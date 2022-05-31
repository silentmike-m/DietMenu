namespace SilentMike.DietMenu.Core.Application.Exceptions.Core;

using System.Runtime.Serialization;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class CoreDataImportEmptyPayloadException : ApplicationException
{
    public override string Code => ErrorCodes.CORE_DATA_IMPORT_EMPTY_PAYLOAD;

    public CoreDataImportEmptyPayloadException()
        : base("Core data to import payload is empty")
    {
    }

    private CoreDataImportEmptyPayloadException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
