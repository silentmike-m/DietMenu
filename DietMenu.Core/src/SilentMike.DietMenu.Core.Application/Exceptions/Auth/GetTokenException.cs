namespace SilentMike.DietMenu.Core.Application.Exceptions.Auth;

using System.Runtime.Serialization;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[Serializable]
public sealed class GetTokenException : ApplicationException
{
    public override string Code => ErrorCodes.GET_TOKEN;

    public GetTokenException()
        : base("Invalid user name or password")
    {
    }

    private GetTokenException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
