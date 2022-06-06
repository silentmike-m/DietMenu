namespace SilentMike.DietMenu.Core.Application.Exceptions;

using System.Runtime.Serialization;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class DietMenuUnauthorizedException : ApplicationException
{
    public override string Code => ErrorCodes.USER_NOT_FOUND;

    public DietMenuUnauthorizedException(Guid familyId, Guid userId)
        : base($"Missing familyId ({familyId}) or userId ({userId})")
    {
        this.Id = Guid.Empty;
    }

    private DietMenuUnauthorizedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}