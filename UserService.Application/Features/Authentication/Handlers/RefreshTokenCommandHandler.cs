using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Common.Services.Token;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Authentication.Handlers;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    ITokenService tokenService,
    IMessageService messageService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<RefreshTokenCommand, Message>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IMessageService _messageService = messageService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Message> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return _messageService.CreateNotAuthorizedMessage("Refresh token não encontrado.");

        var tokenInDb = await _refreshTokenRepository.GetTokenAsync(refreshToken);

        if (tokenInDb == null || tokenInDb.IsExpired() || tokenInDb.IsRevoked == true)
            return _messageService.CreateNotAuthorizedMessage(
                "Token inválido, expirado ou revogado."
            );

        var user = await _userRepository.FindByIdAsync(tokenInDb.UserId);
        if (user == null)
            return _messageService.CreateNotFoundMessage("Usuário não encontrado.");

        var newAccessToken = _tokenService.GenerateToken(user);

        return _messageService.CreateLoginMessage(
            "Access token atualizado com sucesso.",
            newAccessToken
        );
    }
}
