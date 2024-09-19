using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Messages;

namespace UserService.Application.Common.Services.Messages;

public class MessageService : IMessageService
{
    public Message CreateValidationMessage(IEnumerable<string> errors)
    {
        return Message.Error(
            "Validation Error",
            string.Join(", ", errors),
            "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            StatusCodes.Status400BadRequest.ToString()
        );
    }

    public Message CreateNotAuthorizedMessage(string message)
    {
        return Message.Error(
            "Not Authorized",
            message,
            "https://tools.ietf.org/html/rfc7235#section-3.1",
            StatusCodes.Status401Unauthorized.ToString()
        );
    }

    public Message CreateConflictMessage(string message)
    {
        return Message.Error(
            "Conflict",
            message,
            "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            StatusCodes.Status409Conflict.ToString()
        );
    }

    public Message CreateNotFoundMessage(string message)
    {
        return Message.Error(
            "Not Found",
            message,
            "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            StatusCodes.Status404NotFound.ToString()
        );
    }

    public Message CreateSuccessMessage(string message)
    {
        return Message.Success(
            "Success",
            message,
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            StatusCodes.Status200OK.ToString()
        );
    }
}
