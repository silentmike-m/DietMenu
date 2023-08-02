namespace SilentMike.DietMenu.Auth.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common;

[Serializable]
public sealed class DietMenuUnauthorizedException : ApplicationException
{
    public override string Code => ErrorCodes.UNAUTHORIZED;

    public DietMenuUnauthorizedException(Guid familyId, Guid userId)
        : base($"Missing familyId ({familyId}) or userId ({userId})")
    {
    }

    private DietMenuUnauthorizedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
