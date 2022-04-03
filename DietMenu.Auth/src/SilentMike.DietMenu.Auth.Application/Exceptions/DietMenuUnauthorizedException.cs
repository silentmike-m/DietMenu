namespace SilentMike.DietMenu.Auth.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Auth.Application.Common.ApplicationException;

[Serializable]
public sealed class DietMenuUnauthorizedException : ApplicationException
{
    public override string Code => ErrorCodes.USER_NOT_FOUND;

    public DietMenuUnauthorizedException(Guid familyId, Guid userId)
        : base($"Missing portfolioId ({familyId}) or userId ({userId})")
    {
    }

    private DietMenuUnauthorizedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
