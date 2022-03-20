namespace SilentMike.DietMenu.Core.Application.Exceptions.Core;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class CoreDataImportException : ApplicationException
{
    public override string Code => ErrorCodes.CORE_DATA_IMPORT;

    public IDictionary<string, ICollection<ApplicationException>> Exceptions { get; } = new Dictionary<string, ICollection<ApplicationException>>();

    public CoreDataImportException(IDictionary<string, ICollection<ApplicationException>> exceptions)
        : base("Errors have occurred during import core data")
    {
        this.Exceptions = exceptions;
    }

    private CoreDataImportException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
