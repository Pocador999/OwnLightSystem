using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Authentication.Handlers;

public class LogoutCommandHandler(IAuthRepository authRepository, IUserRepository userRepository)
    : IRequestHandler<LogoutCommand, Message>
{
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Message> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);
        if (user == null)
        {
            return Message.NotFound(
                "Not Found",
                "User not found",
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                StatusCodes.Status404NotFound.ToString()
            );
        }

        if (user.IsLogedIn == false)
        {
            return Message.Error(
                "Already logged out",
                $"User {user.Username} is already logged out",
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                StatusCodes.Status400BadRequest.ToString()
            );
        }

        await _authRepository.LogoutAsync(user.Id);

        return Message.Success(
            "Success",
            $"User {user.Username} logged out successfully",
            "https://tools.ietf.org/html/rfc7231#section-6.3.5",
            StatusCodes.Status200OK.ToString()
        );
    }
}
