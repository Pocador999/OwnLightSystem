using MediatR;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Common.Services.Token;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Authentication.Handlers;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    ITokenService tokenService,
    IMessageService messageService
) : IRequestHandler<RefreshTokenCommand, Message>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IMessageService _messageService = messageService;

    public async Task<Message> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (
            refreshToken == null
            || refreshToken.IsRevoked
            || refreshToken.ExpiresAt <= DateTime.UtcNow
        )
        {
            return _messageService.CreateNotAuthorizedMessage(
                "Refresh token inválido ou expirado."
            );
        }

        // Busca o usuário associado ao Refresh Token pelo UserId
        var user =
            await _userRepository.FindByIdAsync(refreshToken.UserId)
            ?? throw new Exception("Usuário não encontrado.");

        // Gera um novo Access Token para o usuário
        var newAccessToken = _tokenService.GenerateToken(user);

        return _messageService.CreateSuccessMessage(
            $"Token atualizado com sucesso. Token: {newAccessToken}"
        );
    }
}
