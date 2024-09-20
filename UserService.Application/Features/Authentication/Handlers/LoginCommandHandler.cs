using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.Authentication.Handlers;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IMessageService messageService,
    IPasswordHasher<Entity.User> passwordHasher,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<LoginCommand, Message>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IPasswordHasher<Entity.User> _passwordHasher = passwordHasher;
    private readonly IMessageService _messageService = messageService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Message> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUsernameAsync(request.Username);
        if (user == null)
            return _messageService.CreateNotFoundMessage("Usuário não encontrado.");

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            request.Password
        );
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return _messageService.CreateNotAuthorizedMessage("Senha incorreta.");

        _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id.ToString());

        var sessionUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(sessionUserId))
            return _messageService.CreateInternalErrorMessage("Erro ao criar a sessão do usuário.");

        await _authRepository.LoginAsync(request.Username, request.Password);

        return _messageService.CreateSuccessMessage("Login realizado com sucesso.");
    }
}
