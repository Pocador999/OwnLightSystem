using Microsoft.AspNetCore.Http;
using UserService.Domain.Entities;
using Msg = UserService.Application.Common.Messages;

namespace UserService.Application.Common.Services.Auth;

public class AuthServices
{
    public static Msg.Messages Authenticate(User user)
    {
        if (user.IsLogedIn == false)
        {
            return Msg.Messages.Error(
                "Unauthorized",
                $"User {user.Username} is not logged in",
                "https://tools.ietf.org/html/rfc7235#section-3.1",
                StatusCodes.Status401Unauthorized.ToString()
            );
        }
        return Msg.Messages.Success(
            "Authorized",
            $"User {user.Username} is logged in",
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            StatusCodes.Status200OK.ToString()
        );
    }
}
