using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Authentication.Handlers;

public class LogoutCommandHandler(
    IAuthRepository authRepository,
    IUserRepository userRepository,
    AuthServices authService,
    IMessageService messageService
) : IRequestHandler<LogoutCommand, Message>
{
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly AuthServices _authServices = authService;
    private readonly IMessageService _messageService = messageService;

    public async Task<Message> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);
        if (user == null)
            return _messageService.CreateNotFoundMessage(
                $"Usuário com id {request.Id} não encontrado"
            );

        if (!user.IsLogedIn)
            return _messageService.CreateNotAuthorizedMessage(
                $"Usuário {user.Username} não está logado"
            );

        await _authServices.LogoutUserAsync(user.Id);
        await _authRepository.LogoutAsync(user.Id);

        return _messageService.CreateSuccessMessage(
            $"Usuário {user.Username} deslogado com sucesso"
        );
    }
}
