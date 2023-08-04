namespace SilentMike.DietMenu.Core.Application.Exceptions;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common;

[Serializable]
public sealed class DietMenuUnauthorizedException : ApplicationException
{
    public override string Code => ErrorCodes.UNAUTHORIZED;

    public DietMenuUnauthorizedException(Guid? familyId, Guid userId)
        : base($"Missing familyId ({familyId}) or userId ({userId})")
        => this.Id = Guid.Empty;

    private DietMenuUnauthorizedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
