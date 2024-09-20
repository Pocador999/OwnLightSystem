using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Authentication.Handlers;

public class LogoutCommandHandler(
    IAuthRepository authRepository,
    IUserRepository userRepository,
    IMessageService messageService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<LogoutCommand, Message>
{
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMessageService _messageService = messageService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Message> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);
        if (user == null)
            return _messageService.CreateNotFoundMessage($"User with id {request.Id} not found");

        if (user.IsLogedIn == false)
            return _messageService.CreateNotAuthorizedMessage(
                $"User {user.Username} is not logged in"
            );

        await _authRepository.LogoutAsync(user.Id);
        _httpContextAccessor.HttpContext.Session.Clear();

        return _messageService.CreateSuccessMessage($"User {user.Username} logged out");
    }
}
