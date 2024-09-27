using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Common.Services.Token;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Common.Services.Auth;

public class AuthServices(
    IMessageService messageService,
    ITokenService tokenService,
    IHttpContextAccessor httpContextAccessor,
    IRefreshTokenRepository refreshTokenRepository
)
{
    private readonly IMessageService _messageService = messageService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

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

    public async Task LogoutUserAsync(Guid userId)
    {
        var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            var tokenInDb = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (tokenInDb != null && tokenInDb.UserId == userId && !tokenInDb.IsRevoked)
                await _refreshTokenRepository.RevokeTokenAsync(tokenInDb);

            // Remover o cookie
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1), // Expirar o cookie
                HttpOnly = true,
                Secure = true,
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                "RefreshToken",
                "",
                cookieOptions
            );
        }
    }

    public async Task<string> LoginUserAsync(User user)
    {
        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.ExpiresAt,
            Secure = true,
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(
            "RefreshToken",
            refreshToken.Token,
            cookieOptions
        );

        return accessToken;
    }
}
