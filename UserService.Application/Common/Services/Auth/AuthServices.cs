using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Messages;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Services.Auth;

public class AuthServices
{
    public static Message Authenticate(User user)
    {
        if (user.IsLogedIn == false)
        {
            return Message.Error(
                "Unauthorized",
                $"User {user.Username} is not logged in",
                "https://tools.ietf.org/html/rfc7235#section-3.1",
                StatusCodes.Status401Unauthorized.ToString()
            );
        }
        return Message.Success(
            "Authorized",
            $"User {user.Username} is logged in",
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            StatusCodes.Status200OK.ToString()
        );
    }
}
