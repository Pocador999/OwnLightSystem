using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Messages;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class DeleteCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteCommand, Messages>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Messages> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteAsync(request.Id);
        if (user == null)
        {
            return Messages.NotFound(
                "not found",
                "User not found",
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                "404"
            );
        }

        var authResult = AuthServices.Authenticate(user);
        if (authResult.StatusCode != StatusCodes.Status200OK.ToString())
            return authResult;

        return Messages.Success(
            "user deleted",
            $"User {user.Username} deleted successfully",
            "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            "200"
        );
    }
}
