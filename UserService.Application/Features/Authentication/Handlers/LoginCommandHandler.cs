using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Common.Services.Token;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.Authentication.Handlers;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IMessageService messageService,
    IPasswordHasher<Entity.User> passwordHasher,
    ITokenService tokenService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<LoginCommand, Message>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IPasswordHasher<Entity.User> _passwordHasher = passwordHasher;
    private readonly IMessageService _messageService = messageService;
    private readonly ITokenService _tokenService = tokenService;
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

        var accessToken = _tokenService.GenerateToken(user);

        var refreshToken = new Entity.RefreshToken
        {
            UserId = user.Id,
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);

        var cookieOptions = new CookieOptions { HttpOnly = true, Expires = refreshToken.ExpiresAt };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(
            "RefreshToken",
            refreshToken.Token,
            cookieOptions
        );

        await _authRepository.LoginAsync(request.Username, request.Password);

        return _messageService.CreateLoginMessage("Login efetuado com sucesso.", accessToken);
    }
}
