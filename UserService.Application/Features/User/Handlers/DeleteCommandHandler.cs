using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class DeleteCommandHandler(
    IUserRepository userRepository,
    IMessageService messageService,
    AuthServices authServices
) : IRequestHandler<DeleteCommand, Message>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMessageService _messageService = messageService;
    private readonly AuthServices _authServices = authServices;

    public async Task<Message> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);
        if (user == null)
            return _messageService.CreateNotFoundMessage("Usuário não encontrado");

        var authResult = _authServices.Authenticate(user);
        if (authResult.StatusCode != StatusCodes.Status200OK.ToString())
            return authResult;

        await _userRepository.DeleteAsync(request.Id);

        return _messageService.CreateSuccessMessage("Usuário deletado com sucesso");
    }
}
