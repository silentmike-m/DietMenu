namespace SilentMike.DietMenu.Core.Infrastructure.Exceptions.EPPlus;

using System;
using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Infrastructure.Common.Constants;

[Serializable]
public sealed class WorksheetNotFoundException : SilentMike.DietMenu.Core.Application.Common.ApplicationException
{
    public override string Code => ErrorCodes.WORKSHEET_NOT_FOUND;

    public WorksheetNotFoundException(string name)
        : base($"Worksheet with name '{name}' has not been found")
    {
        this.Id = Guid.Empty;
    }

    private WorksheetNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
