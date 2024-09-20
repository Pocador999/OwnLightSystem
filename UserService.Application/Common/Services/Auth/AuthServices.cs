using UserService.Application.Common.Services.Messages;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Services.Auth;

public class AuthServices(IMessageService messageService)
{
    private readonly IMessageService _messageService = messageService;

    public Message Authenticate(User user)
    {
        if (user.IsLogedIn == false)
        {
            return _messageService.CreateNotAuthorizedMessage(
                $"Usuário {user.Username} não está autenticado"
            );
        }
        return _messageService.CreateSuccessMessage($"Usuário {user.Username} autenticado");
    }
}
