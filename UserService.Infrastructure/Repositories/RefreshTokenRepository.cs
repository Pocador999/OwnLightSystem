using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories;

public class RefreshTokenRepository(DataContext context) : IRefreshTokenRepository
{
    private readonly DbSet<RefreshToken> _dbSet = context.Set<RefreshToken>();

    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
    {
        var createdToken = await _dbSet.AddAsync(refreshToken);
        await context.SaveChangesAsync();
        return createdToken.Entity;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string refreshToken) =>
        await _dbSet.FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

    public async Task RevokeTokenAsync(string token)
    {
        var refreshToken = await GetByTokenAsync(token);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }
}
