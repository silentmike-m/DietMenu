namespace SilentMike.DietMenu.Core.Application.Exceptions.Families;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common;

[Serializable]
public sealed class FamilyDataImportException : ApplicationException
{
    public override string Code => ErrorCodes.FAMILY_DATA_IMPORT;

    public IDictionary<string, ICollection<ApplicationException>> Exceptions { get; } = new Dictionary<string, ICollection<ApplicationException>>();

    public FamilyDataImportException(IDictionary<string, ICollection<ApplicationException>> exceptions)
        : base("Errors have occurred during import family data")
    {
        this.Exceptions = exceptions;
    }

    private FamilyDataImportException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
