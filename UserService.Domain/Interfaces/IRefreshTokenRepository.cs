using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string token);
}
